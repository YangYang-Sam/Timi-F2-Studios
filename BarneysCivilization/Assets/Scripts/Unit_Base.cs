using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base : MonoBehaviour
{
    public HexCell Cell;
    public CardManager Owner;

    public int Health;
    public int TempHealth;
    public int Level = 1;

    public bool isAlive;
    public bool canMove;

    [SerializeField]
    private Material redMat;

    [SerializeField]
    private Animator[] animator;
    [SerializeField]
    private Transform GraphicTransform;
    [SerializeField]
    private GameObject[] LevelGraphics;

    public List<HexCell> PathCells = new List<HexCell>();
    private void Start()
    {
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        gameObject.name =Owner.camp+" unit:"+ Random.Range(1, 100);
    }

    public int GetTotalHealth()
    {
        return Health + TempHealth;
    }
    private void OnGameStateChange()
    {
        switch (InGameManager.CurrentGameState)
        {       
            case GameStateType.Move:
                if (canMove && Health > Cell.MinUnitAmount && PathCells.Count>0 && PathCells[PathCells.Count-1]!=Cell && canMove)
                {
                    Unit_Base unit = Owner.CreateNewUnit(Cell, Cell.MinUnitAmount);
                    Cell.UnitArrived(unit);
                    ChangeHealth(-Cell.MinUnitAmount);
                    StartCoroutine(MoveProcess());                  
                    
                }
                else
                {
                    Cell.UnitArrived(this);
                }
                break;
            case GameStateType.AfterBattle:
                StartCoroutine(MoveToCenter());
                break;
            case GameStateType.Decision:
                TempHealth = 0;
                canMove = true;
                break;
        }
    }
    public virtual void UnitCreated(HexCell cell, CardManager owner, int health)
    {
        Cell = cell;
        Owner = owner;
        Health = health;

        cell.AddUnit(this);
    }
    public virtual void UnitRemoved()
    {
        Owner.UnitRemoved(this);
        Cell.RemoveUnit(this);
        isAlive = false;
        gameObject.SetActive(false);
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChange;
        Destroy(gameObject,1.5f);
    }
    public virtual void UnitMoveTo(HexCell cell)
    {
        Cell.RemoveUnit(this);
        cell.AddUnit(this);
        Cell = cell;
    }
    public void Death(Unit_Base killer)
    {
        PlayAnimDeath();      
        UnitRemoved();
    }
    public void Merged(Unit_Base MergeToUnit)
    {
        MergeToUnit.ChangeHealth(Health);
        MergeToUnit.TempHealth += TempHealth;
        UnitRemoved();
    }

    Coroutine RotateProcess;
    private IEnumerator MoveProcess()
    {
        RotateProcess = StartCoroutine(RotateTowards(PathCells[0].transform.position));
        HexCell destinyCell = PathCells[PathCells.Count - 1];
        UnitMoveTo(destinyCell);
        SetAnimMoveState(true);

        float speed = PathCells.Count * HexMetrics.innerRadius * 2 / 2.5f;
        while (PathCells.Count > 0)
        {                        
            transform.position += (PathCells[0].transform.position - transform.position).normalized * speed * Time.deltaTime;
            if (Vector3.Distance(PathCells[0].transform.position, transform.position) <= 3)
            {                
                PathCells.RemoveAt(0);
                if (PathCells.Count > 0)
                {
                    if (RotateProcess != null)
                    {
                        StopCoroutine(RotateProcess);
                    }
                    RotateProcess= StartCoroutine(RotateTowards(PathCells[0].transform.position));
                }
            }
            yield return null;
        }
        SetAnimMoveState(false);
        destinyCell.UnitArrived(this);
    }
    private IEnumerator MoveToCenter()
    {
        print("start move");
        SetAnimMoveState(true);
        float distance = Vector3.Distance(transform.position, Cell.transform.position);
        while (distance > 0.2f)
        {
            print(gameObject + "moving: "+distance);
            transform.position = Vector3.MoveTowards(transform.position, Cell.transform.position, 5 * Time.deltaTime);
            distance = Vector3.Distance(transform.position, Cell.transform.position);
            yield return null;
        }
        SetAnimMoveState(false);
    }
    private void SetAnimMoveState(bool isMoving)
    {
        foreach (Animator a in animator)
        {
            a.SetBool("isWalking", isMoving);
        }
    }
    private void PlayAnimDeath()
    {
        foreach (Animator a in animator)
        {
            a.Play("Death");
        }
    }
    public void UpdateDestinyCell()
    {
        if (PathCells.Count > 0)
        {
            if (RotateProcess != null)
            {
                StopCoroutine(RotateProcess);
            }
            RotateProcess = StartCoroutine(RotateTowards(PathCells[0].transform.position));
        }
    }
    private IEnumerator RotateTowards(Vector3 point)
    {
        Vector3 forward = point - GraphicTransform.position;
        forward.y = 0;
        Quaternion TargetRotation = Quaternion.LookRotation(forward);
        float timer = 0.5f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            GraphicTransform.rotation = Quaternion.Slerp(GraphicTransform.rotation, TargetRotation, 0.1f);
            yield return null;
        }
    }
    public virtual void OccupyCell(HexCell cell)
    {
        Cell = cell;
        Owner.OccupyCell(this,cell);
    }
    public void AddTempHealth(int amount)
    {
        TempHealth += amount;
        if (TempHealthChangeEvent != null)
        {
            TempHealthChangeEvent();
        }
    }
    public bool TakeDamage(int amount, Unit_Base source)
    {
        if (TempHealth >= amount)
        {
            TempHealth -= amount; 
            return false;
        }
        else
        {

            amount -= TempHealth;
                   
            ChangeHealth(-amount);
            if (Health <= 0)
            {
                Death(source);
                return true;
            }
            return false;
        }
    }

    public void ChangeHealth(int amount)
    {
        int oldLevel = Level;
        Health += amount;
        if (Health >= 10)
        { 
            Level = 2;
        }
        else if (Health >= 5)
        {
            Level = 1;
        }
        else
        {
            Level = 0;
        }

        if (Level != oldLevel)
        {
            ArtResourceManager.instance.CreateUpgradeEffect(transform.position);
            
            LevelGraphics[oldLevel].SetActive(false);
            LevelGraphics[Level].SetActive(true);
        }
        else
        {
            if (amount > 0)
            {
                ArtResourceManager.instance.CreateHealEffect(transform.position);
            }
        }
        if (HealthChangeEvent != null)
        {
            HealthChangeEvent();
        }
    }
    public event System.Action HealthChangeEvent;
    public event System.Action TempHealthChangeEvent;
}
    

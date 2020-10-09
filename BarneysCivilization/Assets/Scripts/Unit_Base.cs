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
    public bool ForceMove;

    public int LastDamage;
    public int MoveDistance;

    [SerializeField]
    private Animator[] animator;
    [SerializeField]
    private Transform GraphicTransform;
    [SerializeField]
    private GameObject[] LevelGraphics;

    public List<HexCell> TempPathCells = new List<HexCell>();
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
                MoveDistance = 0;
                if (canMove && Health > Cell.MinUnitAmount && PathCells.Count > 0 && PathCells[PathCells.Count - 1] != Cell && (canMove || ForceMove))
                {
                    Unit_Base unit = Owner.CreateNewUnit(Cell, Cell.MinUnitAmount);
                    Cell.UnitArrived(unit);
                    ChangeHealth(-Cell.MinUnitAmount,Vector3.zero);
                    StartCoroutine(MoveProcess());
                }
                else
                {
                    PathCells.Clear();
                    Cell.UnitArrived(this);
                }
                break;
            case GameStateType.AfterBattle:
                if (gameObject.activeSelf)
                {
                    StartCoroutine(MoveToCenter());
                }            
                break;
            case GameStateType.Decision:
                TempHealth = 0;
                canMove = true;
                ForceMove = false;
                TempPathCells.Clear();
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
        if(MergeToUnit.MoveDistance < MoveDistance)
        {
            MergeToUnit.MoveDistance = MoveDistance;
        }
        MergeToUnit.ChangeHealth(Health,transform.position);
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
        MoveDistance = PathCells.Count;

        float distance = Vector3.Distance(PathCells[0].transform.position, transform.position);  
        for (int i = 1; i < PathCells.Count; i++)
        {
            distance += Vector3.Distance(PathCells[i].transform.position, PathCells[i - 1].transform.position);
        }
        float speed = distance / 2.5f;
        while (PathCells.Count > 0)
        {                        
            transform.position += (PathCells[0].transform.position - transform.position).normalized * speed * Time.deltaTime;
            if (Vector3.Distance(PathCells[0].transform.position, transform.position) <= 3)
            {
                Owner.OnUnitVisitCell(this, PathCells[0]);
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
        SetAnimMoveState(true);
        float distance = Vector3.Distance(transform.position, Cell.transform.position);
        while (distance > 0.2f)
        {
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
            if (a.gameObject.activeSelf)
            {
                a.SetBool("isWalking", isMoving);
            }
        }
    }
    private void PlayAnimDeath()
    {
        foreach (Animator a in animator)
        {
            if (a.gameObject.activeSelf)
            {
                a.Play("Death");
            }
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

            LastDamage = Mathf.Min(amount, Health);
            ChangeHealth(-amount,Vector3.zero);
            if (Health <= 0)
            {
                if (DeathEvent != null)
                {
                    DeathEvent(LastDamage,this);
                }
                Death(source);
                return true;
            }
            return false;
        }
    }
    public event System.Action<int,Unit_Base> DeathEvent;

    public void ChangeHealth(int amount, Vector3 FromPosition)
    {
        int oldLevel = Level;
        Health += amount;
        if (Health >= 10)
        { 
            Level = 3;
        }
        else if (Health >= 5)
        {
            Level = 2;
        }
        else
        {
            Level = 1;
        }

        if (Level != oldLevel)
        {
            ArtResourceManager.instance.CreateUpgradeEffect(transform.position);

            LevelGraphics[oldLevel - 1].SetActive(false);
            LevelGraphics[Level - 1].SetActive(true);
        }
        else
        {
            if (amount > 0)
            {
                ArtResourceManager.instance.CreateHealEffect(transform.position);
                ArtResourceManager.instance.CreateTextEffect("+" + amount, transform.position,Color.green,1.2f,2);
            }
        }
        if (HealthChangeEvent != null)
        {
            HealthChangeEvent();
        }
        if (amount > 0)
        {
            if (amount > 3)
            {
                CurveManger.instance.StartNewCurve(FromPosition, transform.position, 1, PlayerController.instance.OrbPrefabs[Owner.camp]);
            }
            else
            {
                CurveManger.instance.StartNewCurve(FromPosition, transform.position, amount, PlayerController.instance.OrbPrefabs[Owner.camp]);
            }
          
        }
    }
    public event System.Action HealthChangeEvent;
    public event System.Action TempHealthChangeEvent;
}
    

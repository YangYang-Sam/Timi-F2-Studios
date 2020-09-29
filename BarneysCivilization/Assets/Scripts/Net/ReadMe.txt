protocol转协议流程：

1、将protoc.exe的文件路径加到环境变量中。

2、在protocol.proto所在文件夹运行如下命令：
protoc ./protocol.proto --csharp_out=./
即可生成对应的Protocol.cs
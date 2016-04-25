# EZReplayManager
EZReplayManager + ProtoBuf
# EZReplayManager数据序列化协议优化(改用 ProtoBuf)
- 开始点Commit
    - Merge branch 'feature/AEX-168-optimize' of http://192.168.199.2:7990/scm/aex/aex into feature/AEX-174-replay-optimize
- EZReplayManager.cs中的两个方法需要修改:
    - protected byte[] SerializeObject (Object2PropertiesMappingListWrapper objectToSerialize)
    - protected Object2PropertiesMappingListWrapper DeSerializeObject (byte[] data)
- 其内部使用的BinaryFormatter在Deserialize时相当耗时,因此尝试改用 ProtoBuf 进行数据序列化
- 对于 ProtoBuf 进行 EZReplayManager 的数据序列化进行的探索是成功的:
    - https://github.com/wang-yichun/EZReplayManager

- 导入 ProtoBuf-net 源码
    - 由于 ProtoBuf-net 的 unity 相关 dll 无法在 IOS 设备上使用,因此采用次源码导入的方式
    - 在 Assets 目录下添加 smcs.rsp 文件, 文件内容 "-unsafe"
- Unity 基础数据类型的 ProtoBuf 定义类型
    - 在 Assets/EZReplayManager/extension/scripts/classes 下建立 PBQuaternion3.cs 和 PBVector3.cs文件

        - 这两个文件代替了原来定义的 SerQuaternion.cs 与 SerVector3.cs
- ISerializable类型转 ProtoBuf 数据类型
    - 去除 ISerializable接口及其接口函数实现.去除对应的构造函数(带有SerializationInfo info, StreamingContext context参数的)
    - 将所有 SerVector3 和 SerQuaternion3 用 PBVector3 和 PBQuaternion 代替
    - 名字空间 using ProtoBuf;
    - ProtoBuf相关特性[ProtoContract],[ProtoMember(1)]...
        - 成员特性参数从1开始,而不是从0开始
    - 加入默认构造函数.这是 ProtoBuf 的要求.
- 处理自包含成员

    - 成员 parentMapping 同样为 Object2PropertiesMapping 类型.测试中发现序列化/反序列化后,其中的引用会发生指向错误(具体原因不知道)
    - 采用忽略序列化引用,而改用序列化该引用在数组中的索引值的办法加以弥补.
    - 代价是会在序列化之前和序列化之后遍历整个数组进行索引和引用的计算转换.
- 处理初始化成员

    - ProtoMember 进行标记的成员不应使用初始化值.
    - 这种初始化方式会使得序列化中为0值的时候被错误的赋值为初始化值-1.
    - 因此,将此处代码的初始化值去掉,而使用自定义的初始化函数进行初始化.把自定义函数加入每个原构造函数中;

- 需要进行 ProtoBuf 化的文件:
    - SavedState.cs
    - Object2PropertiesMapping.cs
    - Object2PropertiesMappingListWrapper.cs
- EZReplayManager.cs修改:
    - 在序列化和反序列化函数中找到BinaryFormatter
    - 对应修改为ProtoBuf.Serializer.Serialize 和 ProtoBuf.Serializer.Deserialize
    - 在之前注意处理自包含成员



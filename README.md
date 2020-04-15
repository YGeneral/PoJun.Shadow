# 绝影框架（Net Core 3.1微服务架构的WebApi框架）

目前本框架支持的数据为：MongoDB、MySql、SqlServer(默认使用的数据库为MongoDB)

## 系统主键图
![系统主键图](https://github.com/YGeneral/PoJun.Shadow/blob/master/doc/img/SystemComponentDiagram.png)

## 代码结构图
![代码结构图](https://github.com/YGeneral/PoJun.Shadow/blob/master/doc/img/CodeStructuralDiagram.png)


# [把绝影变成一个模板，生成自己的项目](https://www.showdoc.cc/392954602410449?page_id=2649330104222489)

# 项目层级介绍

### 1）Shared（共享层）
ASHermed.Shadow.Code - 存放接口返回结果的统一代码
ASHermed.Shadow.ContractModel - 存放接口统一返回结果的Model（无需改动）
ASHermed.Shadow.Entity - 存放与数据库表结构一一对应的实体
ASHermed.Shadow.Enum - 存放所有枚举的地方
ASHermed.Shadow.Exception - 存放所有自定义异常的地方
ASHermed.Shadow.Repository - 存放数据库基础仓储的地方
ASHermed.Shadow.Tools - 存放本系统工具类的地方
﻿
### 2）Model（模型层）
ASHermed.Shadow.Api.ContractModel - 存放项目内所有DTO对象的地方
 
Inside ---- 【对内使用的DTO对象】：
存放仓储层的DTO对象和一些内部使用的DTO对象
External ---- 【对外使用的DTO对象】：
存放Service层、Controller层的DTO对象
Shared ---- 【共享，一般都是存放需要共享的DTO对象或实体】
Framework ---- 【存放所有Framework使用的DTO对象】
﻿
结构示例：
Inside [对内DTO对象]
	Order [模块名称，模块下所有类的命名空间到模块为止]
		Param [存放传入参数的DTO对象，每个模块都有一套，文件结尾以Param命名]
		Model [存放返回结果的DTO对象，每个模块都有一套，文件结尾以Model命名]
		Entity [存放入参或返回结果DTO对象中用到的实体，每个模块都有一套，文件结尾以Entity命名]

External [对外DTO对象]
	Order [模块名称]
		v1 [接口版本，1代表版本号，模块下所有类的命名空间到模块为止]
			Param [存放传入参数的DTO对象，每个模块都有一套，文件结尾以Param命名]
			Model [存放返回结果的DTO对象，每个模块都有一套，文件结尾以Model命名]
			Entity [存放入参或返回结果DTO对象中用到的实体，每个模块都有一套，文件结尾以Entity命名]
﻿
### 3）DataAccess（用户自定义仓储层）
ASHermed.Shadow.Api.IRepository - 存放自定义仓储interface的地方
ASHermed.Shadow.Api.MongoDBRepository - 存放MongoDB自定义数据库仓储的地方
ASHermed.Shadow.Api.MySqlRepository - 存放MySql自定义数据库仓储的地方
ASHermed.Shadow.Api.SqlServerRepository - 存放SqlServer自定义数据库仓储的地方
﻿
### 4）Framework（对外调用层[通过Api调用第三方系统的地方]）
ASHermed.Shadow.BaseFramework - 存放用户调用第三方接口的HttpClientHelp工具类的层
ASHermed.Shadow.IFramework - 存放Framework 下 interface 的地方
ASHermed.Shadow.LogFramework - 存放接口日志记录的地方（无需改动）
ASHermed.SingleSignOnFramework - 存放调用单点登录系统封装的地方
﻿
### 5）Service（业务逻辑层，可以按照模块拆分出多个Service层也可以只用一个Service层然后用文件夹名称区分不同的模块，可以根据个人喜好来定）
ASHermed.Shadow.Api.IService - 存放 Service 下 interface 的地方
ASHermed.Shadow.Api.Service - 存放Service的地方
﻿
### 6）WebApi（WebApi层）
ASHermed.Shadow.WebApi - 存放对外Api的地方

# 框架内置的 ORM 对于MySql 数据库的使用帮助(部分)

```
/// <summary>
/// MySQL框架功能测试（本功能为测试使用）
/// </summary>
/// <param name="param"></param>
/// <returns></returns>
public async Task Test(OperatingUserParam param)
{
    switch (param.Type)
    {
	case 1:
	    #region 新增

	    //普通新增
	    var resut1 = userRepository.initLinq().Insert(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    //普通新增
	    var result2 = await userRepository.initLinq().InsertAsync(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    //新增返回自增ID
	    var result3 = await userRepository.initLinq().InsertReturnIdAsync(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    //新增返回自增ID
	    var result4 = userRepository.initLinq().InsertReturnId(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });

	    //批量新增
	    var addList = new List<User>();
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    addList.Add(new User() { Name = $"PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(SysUtil.RandomString(1, 2)), IsDelete = false, Sex = 1 });
	    var result5 = await userRepository.initLinq().InsertAsync(addList);

	    #endregion
	    break;
	case 2:

	    #region 修改

	    //单条主键更新（并且不更新Age字段）
	    var result6 = await userRepository.initLinq().Filter(x => x.Age).UpdateAsync(new User() { Id = 1, Name = $"修改后的PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}", Sex = 2 });
	    //单条主键更新（并且不更新Age、Sex字段）
	    var result7 = await userRepository.initLinq().Filter(x => x.Age).Filter(x => x.Sex).UpdateAsync(new User() { Id = 2, Name = $"修改后的PoJun{DateTime.Now.ToString("yyyyMMddHHmmss")}" });
	    //动态更新字段
	    var result8 = await userRepository.initLinq().Where(x => x.IsDelete == false && x.Id > 3 && x.Id < 5).Set(x => x.IsDelete, true).UpdateAsync();

	    #endregion

	    break;

	case 3:

	    #region 删除

	    //var result9 = await userRepository.initLinq().Where(x => Operator.In(x.Id, new int[] { 3, 4, 5 })).DeleteAsync();

	    var idlist = new List<int>() { 6, 7 };
	    var result10 = await userRepository.initLinq().Where(x => Operator.In(x.Id, idlist)).DeleteAsync();

	    #endregion

	    break;
	case 4:
	    #region 查询

	    #region 查询单条数据

	    //查询数据库中的第一条记录
	    var result11 = await userRepository.initLinq().SingleAsync();
	    //查询数据库中的最后一条记录
	    var result12 = userRepository.initLinq().OrderByDescending(a => a.Id).Single();
	    //查询数据库中的第一条记录
	    var result13 = userRepository.initLinq().OrderBy("Id").Single();
	    //查询ID等于24的Name字段
	    var result14 = userRepository.initLinq().Where(a => a.Id == 24).Single(s => s.Name);

	    #endregion

	    #region 查询多条数据

	    //查询多条数据
	    var result15 = userRepository.initLinq().Select().ToList();
	    var result16 = (await userRepository.initLinq().Where(a => a.Age > 28).SelectAsync()).ToList();
	    var result17 = userRepository.initLinq().OrderBy(a => a.Id).OrderByDescending(a => a.Name).Select().ToList();
	    var result18 = userRepository.initLinq().Take(4).Select().ToList();
	    var result19 = userRepository.initLinq().Take(4).Skip(2, 2).Select().ToList();
	    //查询多条数据,并且只返回Id、Name两个字段
	    var result20 = userRepository.initLinq().Select(s => new { s.Id, s.Name }).ToList();
	    //查询多条数据,并且只返回UserModel DTO对象 和 Id、Name两个字段
	    var result21 = userRepository.initLinq().Select(s => new UserModel { Id = s.Id, Name = s.Name }).ToList();

	    #endregion

	    #region 分页查询

	    //第一种方式
	    var result22 = userRepository.initLinq().Page(1, 2, out long total1).Select().ToList();
	    var result23 = userRepository.initLinq().Page(2, 2, out long total2).Select().ToList();
	    //第二种方式
	    var id = 24;
	    long total = 0;
	    var result24 = userRepository.initLinq().Where(a => a.Id > id).Page(1, 2, out total).Select();
	    var result25 = userRepository.initLinq().Where(a => a.Id > id).Page(2, 2, out total).Select();
	    //分组分页 
	    var result26 = userRepository.initLinq().GroupBy(a => a.Name).Page(1, 2, out total).Select(s => new { s.Name });
	    var result27 = userRepository.initLinq().GroupBy(a => a.Name).Page(2, 2, out total).Select(s => new { s.Name });

	    #endregion


	    var idlists = new List<int>() { 30, 31, 32, 33, 34, 35 };
	    var result28 = userRepository.initLinq().Where(x => Operator.In(x.Id, idlists)).Select().ToList();

	    #endregion

	    break;
    }
}
```

# 框架内置 ORM 对于MongoDB 数据库的使用帮助

### MongoDB 与 RDBMS Where 语句比较



 操作 | 格式 | 范例 | RDBMS中的类似语句
----    |-------    |--- |---
|等于 | {  key : value  } | db.col.find({"by":"破军"}).pretty() | where by = '破军'|
|小于 | {  key :{$lt: value }} | db.col.find({"likes":{$lt:50}}).pretty() | where likes < 50|
|小于或等于 | {  key :{$lte: value }} | db.col.find({"likes":{$lte:50}}).pretty() | where likes <= 50|
|大于 | {  key :{$gt: value }} | db.col.find({"likes":{$gt:50}}).pretty() | where likes > 50|
|大于或等于 | {  key :{$gte: value }} | db.col.find({"likes":{$gte:50}}).pretty() | where likes >= 50|
|不等于 | {  key :{$ne: value }} | db.col.find({"likes":{$ne:50}}).pretty() | where likes != 50|

### MongoDB日常操作语句

#### mongo 不区分大小写查询 

``` 
{"APIName":{$regex:/cgi-bin\/user\/getuserinfo/,'$options':'i'}}

``` 

#### mongo 正则匹配查询 

``` 
{"RequestBody":{$regex:/B27532093400375009/}}

```

#### mongo 修改

``` 
db.EvaluateRelation.update(               
{"EvaluatorNoId" : "c1363","EvaluatedNoId" : "02255"},  
{ $set: {"IsDeleted" : false} },
{multi:true}
)
``` 

#### mongo 删除多条

``` 
db.AddedCount.remove(               
{"_id" : {$in:["c0553.3","02669.3"]}}, 
{multi:true}
)
``` 

#### 建立索引 

``` 
db.APILogs.createIndex({"RequestTime":-1})
db.APILogs.ensureIndex({"APIName":1,"IsError":1})

```

### MongoDB 在C#里面的用法

#### C#中模糊查询 

``` 
var filter = Filter.Or(
    Filter.Where(x => x.Email.Contains(keyWord.ToLower())),
    Filter.Where(x => x.WorkerUserId.Contains(keyWord.ToLower())));

``` 

#### C#中不区分大小写模糊查询(非标准写法) 

``` 
var filter = Filter.Regex(x => x.EnglishName, new MongoDB.Bson.BsonRegularExpression(keyWord, "i"));
``` 

#### C#中不区分大小写模糊查询(标准写法) 

``` 
var filter = Filter.Or(
         Filter.Regex(x => x.EnglishName, new BsonRegularExpression(new Regex(keyWord, RegexOptions.IgnoreCase))),
         Filter.Regex(x => x.EnglishName, new BsonRegularExpression(keyWord)));
``` 

#### C#分页查询 

``` 
int skip = (pageNo - 1) * pageSize;

await this.GetListAsync(filter: _filter, sort: Sort.Ascending(x => x.HireDate), limit: pageSize, skip: skip);
``` 

#### C#单条新增 

``` 
//无状态
await this.InsertAsync(surveyAnswers);
//有状态
await this.InsertAsync(surveyAnswers, writeConcern: WriteConcern.Acknowledged);
``` 

#### C#查询主库

``` 
await this.CountAsync(filter: filter, readPreference: ReadPreference.PrimaryPreferred);
``` 

#### C#批量新增

``` 
//无状态
this.InsertBatchAsync(surveyAnswers);
//有状态
this.InsertBatchAsync(surveyAnswers, writeConcern: WriteConcern.Acknowledged);
``` 

#### C#更新状态

``` 
this.FindOneAndUpdateAsync(filter, update);
``` 

#### C#自增列实现

``` 
    public class Question : IAutoIncr<int>
    {
        [BsonId]
        public int ID { get; set; }
	}
``` 

#### C# string为主键，自动赋值

``` 
    public class RecommendRelation : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
	}
``` 

#### C# 查询部分字段

```
            var filter = Filter.And(Filter.In(x => x.ID, ids));

            People people = new People();

            var projection = Projection
                .Include(nameof(people.ID))
                .Include(nameof(people.EN_Sup_Org_Name))
                .Include(nameof(people.CN_Sup_Org_Name));

            return this.GetListAsync(filter: filter, projection: projection);
``` 

#### C#更新多层嵌套

``` 

{
    "TableName" : "frtest66",
    "MenuName" : "test666",
    "DOAdmin_MenuId" : "676db672-212f-4876-b164-22ded65f4cbd",
    "PermissionList" : [ 
        {
            "PmsID" : "0d27cbe577764b72a160bc84c8aa2203",
            "PmsTag" : [ 
                "64ef17e8-37fb-47fa-b630-1682c137e463"
            ],
            "Remark" : "test",
            "OperateType" : [ 
                1, 
                2, 
                4
            ],
            "Status" : 2,
            "IsFullData" : false,
            "PmsInfos" : [ 
                {
                    "ColumnName" : "Name",
                    "ColumnType" : 1,
                    "ColumnValue" : "张三"
                }
            ]
        }
    ]
}
更新Status字段：通过占位符"$"实现
"update" : { "$set" : { "PermissionList.$.Status" : 1 } }

实现代码:
var filter = Filter.And(
                Filter.Eq(nameof(Sys_SingleTableTemplate.PermissionList) + "." + nameof(Sys_Permission.PmsID), param.PermissionID));
            var update = Update
               .Set(nameof(Sys_SingleTableTemplate.PermissionList) + ".$." + nameof(Sys_Permission.Status), param.Sys_DataState);

  return await this.FindOneAndUpdateAsync(filter, update, writeConcern:      WriteConcern.Acknowledged);
``` 

#### C#更新单个实体

``` 

/// <summary>
/// 投放ID集合
/// </summary>
public List<MPPutInInfo> PICMList { get; set; }

/// <summary>
/// 更新券规则投放数据
/// </summary>
/// <param name="mppicrID">后台发券规则ID</param>
/// <param name="mpPutInInfo">投放ID集合</param>
/// <param name="title">后台发券规则标题</param>
/// <param name="updateID">最后修改人ID</param>
/// <param name="updateName">最后修改人姓名</param>
/// <returns></returns>
public Task UpdateCouponPutIn(long mppicrID, MPPutInInfo mpPutInInfo, string title, long updateID, string updateName)
{
	var filter = Filter.And(
	Filter.Eq(nameof(MPPutInCouponRule.ID), mppicrID));

	var update = Update
	.Push(x => x.PICMList, mpPutInInfo)
	.Set(nameof(MPPutInCouponRule.Title), title)
	.Set(nameof(MPPutInCouponRule.UpdateID), updateID)
	.Set(nameof(MPPutInCouponRule.UpdateName), updateName)
	.Set(nameof(MPPutInCouponRule.UpdateTime), DateTime.Now);
	return this.FindOneAndUpdateAsync(filter, update);
}

``` 

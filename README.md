# 绝影框架（Net Core 2.2微服务架构的WebApi框架）

目前本框架支持的数据为：MongoDB、MySql、SqlServer(默认使用的数据库为MongoDB)

## 系统主键图
![系统主键图](https://github.com/YGeneral/PoJun.Shadow/blob/master/doc/img/SystemComponentDiagram.png)

## 代码结构图
![代码结构图](https://github.com/YGeneral/PoJun.Shadow/blob/master/doc/img/CodeStructuralDiagram.png)


# [把绝影变成一个模板，生成自己的项目](https://mp.weixin.qq.com/s/dx7u5ZNYe4SpDAkWOtxHkA)


# MongoDB 使用帮助

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

using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoJun.Shadow.Api.IService.Test;
using PoJun.Shadow.Api.IRepository.Test;
using PoJun.Shadow.Api.ContractModel.External.Test;
using SqlSugar;
using PoJun.Shadow.Entity.Test;

namespace PoJun.Shadow.Api.Service.Test
{
    /// <summary>
	/// 用户表服务
	/// 创建人：杨江军
	/// 创建时间：2020/4/7 12:01:43
	/// </summary>
	public class UserService : IUserService
    {
        /// <summary>
        /// 用户表仓储层
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="_userRepository"></param>
        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        /// <summary>
        /// MySQL框架功能测试（本功能为测试使用）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task Test(OperatingUserParam param)
        {
            //完整版示例代码： https://www.donet5.com/Home/Doc?typeId=1228

            switch (param.Type)
            {
                case 1:
                    #region 新增

                    //普通新增
                    await userRepository.InsertAsync(new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 });


                    //批量新增
                    var addList = new List<User>
                    {
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 },
                        new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 }
                    };
                    await userRepository.InsertRangeAsync(addList);

                    //返回自增ID的新增
                    var id = await userRepository.InsertReturnIdentityAsync(new User() { Name = $"PoJun{DateTime.Now:yyyyMMddHHmmss}{PoJun.Util.Helpers.Id.GetGuidBy6()}", Age = Convert.ToInt32(PoJun.Util.Helpers.Random.RandomString(1, 2)), IsDelete = false, Sex = 1 });



                    #endregion
                    break;
                case 2:

                    #region 修改

                    //单条主键更新（只更新Age字段）
                    var result1 = await userRepository.UpdateAsync(x => new User() { Age = 99 }, y => y.Id == 6);



                    #endregion

                    break;

                case 3:

                    #region 删除

                    //var result9 = await userRepository.initLinq().Where(x => Operator.In(x.Id, new int[] { 3, 4, 5 })).DeleteAsync();

                    //var idlist = new List<int>() { 6, 7 };
                    //var result10 = await userRepository.initLinq().Where(x => Operator.In(x.Id, idlist)).DeleteAsync();

                    #endregion

                    break;
                case 4:
                    #region 查询


                    var data1 = userRepository.GetById(1);//根据id查询
                    var data2 = userRepository.GetList();//查询所有
                    var data3 = userRepository.GetList(it => it.Id == 1); //根据条件查询
                    var data4 = userRepository.GetSingle(it => it.Id == 1);
                    var p = new PageModel() { PageIndex = 1, PageSize = 2 };

                    //精确查询
                    var data5 = userRepository.GetPageList(it => it.Name == "PoJun20210604164838LioUzT", p);

                    //模糊查询
                    var data6 = userRepository.GetPageList(it => it.Name.Contains("0604164838"), p, it => it.Name, OrderByType.Asc);

                    List<IConditionalModel> conModels = new()
                    {
                        new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = "1" }//id=1
                    };
                    var data7 = userRepository.GetPageList(conModels, p, it => it.Name, OrderByType.Asc);
                    var data8 = userRepository.AsQueryable().Where(x => x.Id == 1).ToList();

                    #endregion

                    break;
            }
        }
    }


}

using PoJun.Dapper;
using PoJun.Shadow.Api.ContractModel.External.Test;
using PoJun.Shadow.Api.ContractModel.External.Test.v1;
using PoJun.Shadow.Api.IRepository.Test;
using PoJun.Shadow.Api.IService.Test;
using PoJun.Shadow.Entity.Test;
using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private IUserRepository userRepository;

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


    }


}

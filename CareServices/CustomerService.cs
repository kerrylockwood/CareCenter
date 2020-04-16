using CareData;
using CareModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class CustomerService
    {
        private readonly string _userId;

        public CustomerService(string userId)
        {
            _userId = userId;
        }

        public CustBarCode ValidateCustBarCode(int barCode)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.BarCodes.Count(e => e.BarCodeNumber == barCode) == 0)
                {
                    return null;
                }
                var entity =
                    ctx
                        .BarCodes
                        .Single(e => e.BarCodeNumber == barCode);
                return
                    new CustBarCode
                    {
                        BarCodeId = entity.BarCodeId,
                        BarCodeNumber = entity.BarCodeNumber
                    };
            }
        }

        public IEnumerable<CustList> GetCusts()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Customers
                        .Select(
                            e =>
                                new CustList
                                {
                                    CustomerId = e.CustomerId,
                                    BarCodeNumber = e.BarCode.BarCodeNumber,
                                    FirstName = e.FirstName,
                                    LastName = e.LastName,
                                    City = e.City,
                                    State = e.State,
                                    ZipCode = e.ZipCode,
                                    Phone = e.Phone
                                }
                        );

                return query.ToArray();
            }
        }

        public CustDetail GetCustById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Customers
                        .Single(e => e.CustomerId == id);
                return
                    new CustDetail
                    {
                        CustomerId = entity.CustomerId,
                        BarCodeId = entity.BarCodeId,
                        BarCodeNumber = (entity.BarCodeId == null) ? 0 : entity.BarCode.BarCodeNumber,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Address = entity.Address,
                        City = entity.City,
                        State = entity.State,
                        ZipCode = entity.ZipCode,
                        Phone = entity.Phone,
                        Email = entity.Email,
                        NumberKids = entity.NumberKids,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public CustDetail GetCustByBarCodeId(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.Customers.Count(e => e.BarCodeId == id) == 0)
                {
                    return new CustDetail();
                }
                var entity =
                    ctx
                        .Customers
                        .Single(e => e.BarCodeId == id);
                return
                    new CustDetail
                    {
                        CustomerId = entity.CustomerId,
                        BarCodeId = entity.BarCodeId,
                        BarCodeNumber = entity.BarCode.BarCodeNumber,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Address = entity.Address,
                        City = entity.City,
                        State = entity.State,
                        ZipCode = entity.ZipCode,
                        Phone = entity.Phone,
                        Email = entity.Email,
                        NumberKids = entity.NumberKids,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public int CreateCust(CustCreate model)
        {
            var entity =
                new Customer()
                {
                    //BarCodeId = model.BarCodeId,  // this must be null if BarCode is 0
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    Phone = model.Phone,
                    Email = model.Email,
                    NumberKids = model.NumberKids,
                    CreateBy = _userId,
                    CreateAt = DateTimeOffset.Now
                };
            if (model.BarCodeId != 0)
            {
                entity.BarCodeId = model.BarCodeId;
            }
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Customers.Add(entity);
                ctx.SaveChanges();
                var id = entity.CustomerId;
                return id;
            }
        }

        public bool UpdateCust(CustUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Customers
                        .Single(e => e.CustomerId == model.CustomerId);
                entity.BarCodeId = model.BarCodeId;
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Address = model.Address;
                entity.City = model.City;
                entity.State = model.State;
                entity.ZipCode = model.ZipCode;
                entity.Phone = model.Phone;
                entity.Email = model.Email;
                entity.NumberKids = model.NumberKids;

                bool rtnBool = true;

                try { ctx.SaveChanges(); }
                catch { rtnBool = false; }

                return rtnBool;
            }
        }

        public bool DeleteCust(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Customers
                        .Single(e => e.CustomerId == id);

                ctx.Customers.Remove(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }
    }
}

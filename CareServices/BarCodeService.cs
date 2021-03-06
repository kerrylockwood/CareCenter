﻿using CareData;
using CareModels.BarCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class BarCodeService
    {
        private readonly string _userId;

        public BarCodeService(string userId)
        {
            _userId = userId;
        }

        public bool CreateBarCode(BarCodeCreate model)
        {
            var entity =
                new BarCode()
                {
                    BarCodeNumber = model.BarCodeNumber,
                    CreateBy = _userId,
                    CreateAt = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.BarCodes.Add(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }

        public IEnumerable<BarCodeList> GetBarCodes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .BarCodes
                        .OrderBy(e => e.BarCodeNumber)
                        .Select(
                            e =>
                                new BarCodeList
                                {
                                    BarCodeId = e.BarCodeId,
                                    BarCodeNumber = e.BarCodeNumber
                                }
                        );
                BarCodeList newBarCode = new BarCodeList
                {
                    BarCodeId = 0,
                    BarCodeNumber = 0,
                };
                List<BarCodeList> queryList = query.ToList();

                return queryList.ToArray();
            }
        }

        public BarCodeDetail GetBarCodeById(int barCodeId)
        {
            if (barCodeId == 0)
            {
                return
                    new BarCodeDetail
                    {
                        BarCodeId = 0,
                    };
            }
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .BarCodes
                        .Single(e => e.BarCodeId == barCodeId);
                return
                    new BarCodeDetail
                    {
                        BarCodeId = entity.BarCodeId,
                        BarCodeNumber = entity.BarCodeNumber,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
            };
            }
        }

        public BarCodeDetail GetBarCodeByBarCode(int barCodeNum)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.BarCodes.Count(e => e.BarCodeNumber == barCodeNum)
                    == 0)
                {
                    return
                    new BarCodeDetail
                    {
                        BarCodeId = 0,
                    };
                }
                var entity =
                    ctx
                        .BarCodes
                        .Single(e => e.BarCodeNumber == barCodeNum);
                return
                    new BarCodeDetail
                    {
                        BarCodeId = entity.BarCodeId,
                        BarCodeNumber = entity.BarCodeNumber,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public bool DeleteBarCode(int barCodeId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .BarCodes
                        .Single(e => e.BarCodeId == barCodeId);

                ctx.BarCodes.Remove(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }
    }
}

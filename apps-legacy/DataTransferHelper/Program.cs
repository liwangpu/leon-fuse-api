//using DataTransferHelper.Transfer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataTransferHelper.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataTransferHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TransferData().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine("程序异常:{0}", ex.Message);
            }
            Console.WriteLine("数据迁移完毕...");
            Console.Read();
        }

        static async Task TransferData()
        {
            //await SolutionTransfer.Transfer("MGUZMA909KQEMJ", "PPUJPG33399YP0");
            var bamboo_admin = "8WUKG1959M3Q60";
            var bamboo_organId = "MGUZMA909KQEMJ";
            var ehome_admin = "8WUJPZKM38NM9K";
            var echome_organId = "40U0E94V9G88GK";

            using (var srcDb = new SrcContext())
            using (var destDb = new DestContext())
            {
                ////1=>转移files
                //{
                //    var srcFileIds = srcDb.Files.Where(x => x.OrganizationId == bamboo_organId).Select(x => x.Id).ToList();
                //    var len = srcFileIds.Count;
                //    for (int i = 0; i < len; i++)
                //    {
                //        var sid = srcFileIds[i];
                //        var fs = srcDb.Files.Where(x => x.Id == sid).First();
                //        fs.Creator = ehome_admin;
                //        fs.Modifier = ehome_admin;
                //        fs.CreatedTime = DateTime.Now;
                //        fs.ModifiedTime = fs.CreatedTime;
                //        fs.OrganizationId = echome_organId;
                //        destDb.Files.Add(fs);
                //        if (i % 200 == 0)
                //        {
                //            destDb.SaveChanges();
                //            Console.WriteLine("save {0} / {1}", i, len);
                //        }
                //        else if (i == len - 1)
                //        {
                //            Console.WriteLine("final save {0} / {1}", i, len);
                //            destDb.SaveChanges();
                //        }
                //        else { }
                //    }
                //}

                ////2=>转移materials
                //{
                //    var srcIds = srcDb.Materials.Where(x => x.OrganizationId == bamboo_organId).Select(x => x.Id).ToList();
                //    var len = srcIds.Count;
                //    for (int i = 0; i < len; i++)
                //    {
                //        var sid = srcIds[i];
                //        var nitem = srcDb.Materials.Where(x => x.Id == sid).First();
                //        nitem.Creator = ehome_admin;
                //        nitem.Modifier = ehome_admin;
                //        nitem.CreatedTime = DateTime.Now;
                //        nitem.ModifiedTime = nitem.CreatedTime;
                //        nitem.OrganizationId = echome_organId;
                //        if (!string.IsNullOrWhiteSpace(nitem.FileAssetId))
                //        {
                //            var existFile = destDb.Files.Any(x => x.Id == nitem.FileAssetId);
                //            if (!existFile)
                //            {
                //                nitem.FileAssetId = null;
                //            }
                //        }
                //        else
                //        {
                //            nitem.FileAssetId = null;
                //        }

                //        destDb.Materials.Add(nitem);
                //        if (i % 200 == 0)
                //        {
                //            destDb.SaveChanges();
                //            Console.WriteLine("save {0} / {1}", i, len);
                //        }
                //        else if (i == len - 1)
                //        {
                //            Console.WriteLine("final save {0} / {1}", i, len);
                //            destDb.SaveChanges();
                //        }
                //        else { }
                //    }
                //}


                ////3=>texture
                //{
                //    var srcIds = srcDb.Textures.Select(x => x.Id).ToList();
                //    var len = srcIds.Count;
                //    for (int i = 0; i < len; i++)
                //    {
                //        var sid = srcIds[i];
                //        var nitem = srcDb.Textures.Where(x => x.Id == sid).First();
                //        nitem.Creator = ehome_admin;
                //        nitem.Modifier = ehome_admin;
                //        nitem.CreatedTime = DateTime.Now;
                //        nitem.ModifiedTime = nitem.CreatedTime;
                //        nitem.OrganizationId = echome_organId;
                //        if (!string.IsNullOrWhiteSpace(nitem.FileAssetId))
                //        {
                //            var existFile = destDb.Files.Any(x => x.Id == nitem.FileAssetId);
                //            if (!existFile)
                //            {
                //                nitem.FileAssetId = null;
                //            }
                //        }
                //        else
                //        {
                //            nitem.FileAssetId = null;
                //        }
                //        destDb.Textures.Add(nitem);
                //        if (i % 200 == 0)
                //        {
                //            destDb.SaveChanges();
                //            Console.WriteLine("save {0} / {1}", i, len);
                //        }
                //        else if (i == len - 1)
                //        {
                //            Console.WriteLine("final save {0} / {1}", i, len);
                //            destDb.SaveChanges();
                //        }
                //        else { }
                //    }
                //}

                ////4=>StaticMeshs
                //{
                //    var srcIds = srcDb.StaticMeshs.Where(x => x.OrganizationId == bamboo_organId).Select(x => x.Id).ToList();
                //    var len = srcIds.Count;
                //    for (int i = 0; i < len; i++)
                //    {
                //        var sid = srcIds[i];
                //        var nitem = srcDb.StaticMeshs.Where(x => x.Id == sid).First();
                //        nitem.Creator = ehome_admin;
                //        nitem.Modifier = ehome_admin;
                //        nitem.CreatedTime = DateTime.Now;
                //        nitem.ModifiedTime = nitem.CreatedTime;
                //        nitem.OrganizationId = echome_organId;
                //        if (!string.IsNullOrWhiteSpace(nitem.FileAssetId))
                //        {
                //            var existFile = destDb.Files.Any(x => x.Id == nitem.FileAssetId);
                //            if (!existFile)
                //            {
                //                nitem.FileAssetId = null;
                //            }
                //        }
                //        else
                //        {
                //            nitem.FileAssetId = null;
                //        }
                //        destDb.StaticMeshs.Add(nitem);
                //        if (i % 200 == 0)
                //        {
                //            destDb.SaveChanges();
                //            Console.WriteLine("save {0} / {1}", i, len);
                //        }
                //        else if (i == len - 1)
                //        {
                //            Console.WriteLine("final save {0} / {1}", i, len);
                //            destDb.SaveChanges();
                //        }
                //        else { }
                //    }
                //}


                //5=>分类
                //{
                //    var orgMatCats = destDb.AssetCategories.Where(x => x.Type == "material").ToList();
                //    destDb.AssetCategories.RemoveRange(orgMatCats);
                //    var orgMatTree = destDb.AssetCategoryTrees.Where(x => x.NodeType == "material").ToList();
                //    destDb.AssetCategoryTrees.RemoveRange(orgMatTree);
                //    destDb.SaveChanges();
                //    //cat
                //    {
                //        var srcIds = srcDb.AssetCategories.Where(x => x.OrganizationId == bamboo_organId && x.Type == "material").Select(x => x.Id).ToList();
                //        var len = srcIds.Count;
                //        for (int i = 0; i < len; i++)
                //        {
                //            var sid = srcIds[i];
                //            var nitem = srcDb.AssetCategories.Where(x => x.Id == sid).First();
                //            nitem.Creator = ehome_admin;
                //            nitem.Modifier = ehome_admin;
                //            nitem.CreatedTime = DateTime.Now;
                //            nitem.ModifiedTime = nitem.CreatedTime;
                //            nitem.OrganizationId = echome_organId;
                //            destDb.AssetCategories.Add(nitem);
                //            if (i % 200 == 0)
                //            {
                //                destDb.SaveChanges();
                //                Console.WriteLine("save {0} / {1}", i, len);
                //            }
                //            else if (i == len - 1)
                //            {
                //                Console.WriteLine("final save {0} / {1}", i, len);
                //                destDb.SaveChanges();
                //            }
                //            else { }
                //        }
                //    }

                //    //cat tree
                //    {
                //        var srcIds = srcDb.AssetCategoryTrees.Where(x => x.OrganizationId == bamboo_organId && x.NodeType == "material").Select(x => x.Id).ToList();
                //        var len = srcIds.Count;
                //        for (int i = 0; i < len; i++)
                //        {
                //            var sid = srcIds[i];
                //            var nitem = srcDb.AssetCategoryTrees.Where(x => x.Id == sid).First();
                //            nitem.OrganizationId = echome_organId;
                //            nitem.RootOrganizationId = echome_organId;
                //            destDb.AssetCategoryTrees.Add(nitem);
                //            if (i % 200 == 0)
                //            {
                //                destDb.SaveChanges();
                //                Console.WriteLine("save {0} / {1}", i, len);
                //            }
                //            else if (i == len - 1)
                //            {
                //                Console.WriteLine("final save {0} / {1}", i, len);
                //                destDb.SaveChanges();
                //            }
                //            else { }
                //        }
                //    }
                //}




                ////6=>转移product
                //{
                //    var srcIds = srcDb.Products.Where(x => x.OrganizationId == bamboo_organId).Select(x => x.Id).ToList();
                //    var len = srcIds.Count;
                //    for (int i = 0; i < len; i++)
                //    {
                //        var sid = srcIds[i];
                //        var nitem = srcDb.Products.Include(x => x.Specifications).Where(x => x.Id == sid).First();
                //        nitem.Creator = ehome_admin;
                //        nitem.Modifier = ehome_admin;
                //        nitem.CreatedTime = DateTime.Now;
                //        nitem.ModifiedTime = nitem.CreatedTime;
                //        nitem.OrganizationId = echome_organId;
                //        if (nitem.Specifications != null)
                //        {
                //            for (int idx = nitem.Specifications.Count - 1; idx >= 0; idx--)
                //            {
                //                var sitem = nitem.Specifications[idx];
                //                sitem.Creator = ehome_admin;
                //                sitem.Modifier = ehome_admin;
                //                sitem.CreatedTime = DateTime.Now;
                //                sitem.ModifiedTime = nitem.CreatedTime;
                //                sitem.OrganizationId = echome_organId;
                //            }
                //        }

                //        destDb.Products.Add(nitem);
                //        if (i % 200 == 0)
                //        {
                //            destDb.SaveChanges();
                //            Console.WriteLine("save {0} / {1}", i, len);
                //        }
                //        else if (i == len - 1)
                //        {
                //            Console.WriteLine("final save {0} / {1}", i, len);
                //            destDb.SaveChanges();
                //        }
                //        else { }
                //    }
                //}










                ////Files
                //foreach (var item in srcDb.Files)
                //{
                //    var exist = destDb.Files.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.Files.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"Files"} 迁移完毕");

                ////AssetCategories
                //foreach (var item in srcDb.AssetCategories)
                //{
                //    var exist = destDb.AssetCategories.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.AssetCategories.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"AssetCategories"} 迁移完毕");

                ////AssetCategoryTrees
                //foreach (var item in srcDb.AssetCategoryTrees)
                //{
                //    var exist = destDb.AssetCategoryTrees.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.AssetCategoryTrees.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"AssetCategoryTrees"} 迁移完毕");

                ////Permissions
                //foreach (var item in srcDb.Permissions)
                //{
                //    var exist = destDb.Permissions.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.Permissions.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"Permissions"} 迁移完毕");

                ////PermissionTrees
                //foreach (var item in srcDb.PermissionTrees)
                //{
                //    var exist = destDb.PermissionTrees.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.PermissionTrees.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"PermissionTrees"} 迁移完毕");

                ////ProductGroups
                //foreach (var item in srcDb.ProductGroups)
                //{
                //    var exist = destDb.ProductGroups.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.ProductGroups.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"ProductGroups"} 迁移完毕");

                ////ProductReplaceGroups
                //foreach (var item in srcDb.ProductReplaceGroups)
                //{
                //    var exist = destDb.ProductReplaceGroups.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.ProductReplaceGroups.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"ProductReplaceGroups"} 迁移完毕");


                ////ProductReplaceGroups
                //foreach (var item in srcDb.ResourcePermissions)
                //{
                //    var exist = destDb.ResourcePermissions.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.ResourcePermissions.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"ResourcePermissions"} 迁移完毕");





                ////Products
                //foreach (var item in srcDb.Products)
                //{
                //    var exist = destDb.Products.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.Products.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"Products"} 迁移完毕");

                ////Products
                //foreach (var item in srcDb.ProductSpec)
                //{
                //    var exist = destDb.ProductSpec.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.ProductSpec.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"ProductSpec"} 迁移完毕");


                ////StaticMeshs
                //foreach (var item in srcDb.StaticMeshs)
                //{
                //    var exist = destDb.StaticMeshs.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.StaticMeshs.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"StaticMeshs"} 迁移完毕");

                ////Maps
                //foreach (var item in srcDb.Maps)
                //{
                //    var exist = destDb.Maps.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.Maps.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"Maps"} 迁移完毕");

                //////Layouts
                ////foreach (var item in srcDb.Layouts)
                ////{
                ////    var exist = destDb.Layouts.Any(x => x.Id == item.Id);
                ////    if (!exist)
                ////    {
                ////        destDb.Layouts.Add(item);
                ////    }
                ////}
                ////destDb.SaveChanges();
                ////Console.WriteLine($"{"Layouts"} 迁移完毕");

                //////Solutions
                ////foreach (var item in srcDb.Solutions)
                ////{
                ////    var exist = destDb.Solutions.Any(x => x.Id == item.Id);
                ////    if (!exist)
                ////    {
                ////        destDb.Solutions.Add(item);
                ////    }
                ////}
                ////destDb.SaveChanges();
                ////Console.WriteLine($"{"Solutions"} 迁移完毕");


                ////Packages
                //foreach (var item in srcDb.Packages)
                //{
                //    var exist = destDb.Packages.Any(x => x.Id == item.Id);
                //    if (!exist)
                //    {
                //        destDb.Packages.Add(item);
                //    }
                //}
                //destDb.SaveChanges();
                //Console.WriteLine($"{"Packages"} 迁移完毕");



            }

        }
    }
}

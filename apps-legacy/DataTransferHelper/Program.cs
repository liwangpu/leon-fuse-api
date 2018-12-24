//using DataTransferHelper.Transfer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataTransferHelper.Contexts;
using System.Linq;
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

            using (var srcDb = new SrcContext())
            using (var destDb = new DestContext())
            {

















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

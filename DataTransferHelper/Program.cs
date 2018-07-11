using DataTransferHelper.Transfer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            Console.WriteLine("执行完毕...");
            Console.Read();
        }

        static async Task TransferData()
        {
            //await SolutionTransfer.Transfer("MGUZMA909KQEMJ", "PPUJPG33399YP0");




        }
    }
}

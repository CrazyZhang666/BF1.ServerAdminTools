using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.GameImage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Test;

internal class WindowImgTests : IMsgCall
{
    public void Error(string data, Exception e)
    {
    }

    public void Info(string data)
    {
    }

    [Test]
    public void TestPic()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        var bitmap = GameWindowImg.GetWindow();
        bitmap.Save("test.png");
    }

    [Test]
    public void TestToM()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowMessage.ToM();
    }

    [Test]
    public void TestToServerList()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowMessage.ToServerList();
    }

    [Test]
    public void TestToServerList1()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowMessage.ToServerList1();
    }

    [Test]
    public void TestToServer()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowMessage.ToServer();
    }

    [Test]
    public void TestJoinServer()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowMessage.JoinServer();
    }

    [Test]
    public void TestCV1()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowOpenCV.Test1();
    }

    [Test]
    public void TestCV2()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowOpenCV.Test2();
    }

    [Test]
    public void TestCV3()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowOpenCV.Test3();
    }

    [Test]
    public void TestJoin() 
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;

        WindowMessage.ToM();
        int a = 0;
        do
        {
            if (WindowOpenCV.Test1())
                break;
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
        if (a >= 5)
        {
            Assert.Fail("Window Error");
        }

        WindowMessage.ToServerList();
        Thread.Sleep(500);
        WindowMessage.ToServerList1();
        Thread.Sleep(1000);
        a = 0;
        do
        {
            if (WindowOpenCV.Test2())
                break;
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
        if (a >= 5)
        {
            Assert.Fail("Window Error");
        }

        WindowMessage.ToServer();
        Thread.Sleep(1000);
        a = 0;
        do
        {
            if (WindowOpenCV.Test3())
                break;
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
        if (a >= 5)
        {
            Assert.Fail("Window Error");
        }

        WindowMessage.JoinServer();
    }

    [Test]
    public void TestCV4()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        int a = 0;
        do
        {
            if (WindowOpenCV.Error1())
            {
                WindowMessage.Ok();
            }
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
    }

    [Test]
    public void TestCV5()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        int a = 0;
        do
        {
            if (WindowOpenCV.Error2())
            {
                WindowMessage.Online();
            }
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
    }
    [Test]
    public void TestCV6()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        int a = 0;
        do
        {
            if (WindowOpenCV.Error3())
            {
                WindowMessage.Ok();
            }
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
    }
    [Test]
    public void TestCV7()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        int a = 0;
        do
        {
            if (WindowOpenCV.Error4())
            {
                WindowMessage.Ok();
            }
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
    }
    [Test]
    public void TestCV8()
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        WindowOpenCV.Info1();
    }

    [Test]
    public void TestCheck() 
    {
        Core.Init(this);
        var res = Core.HookInit();
        Assert.IsTrue(res);
        if (!res)
            return;
        bool isOut = true;
        while (true)
        {
            Thread.Sleep(10000);
            Console.WriteLine("Start");
            if (WindowOpenCV.Error1())
            {
                WindowMessage.Ok();
                isOut = true;
                continue;
            }
            else if (WindowOpenCV.Error2())
            {
                WindowMessage.Online();
                isOut = true;
                continue;
            }
            else if (WindowOpenCV.Error3())
            {
                WindowMessage.Ok();
                isOut = true;
                continue;
            }
            else if (WindowOpenCV.Error4())
            {
                WindowMessage.Online();
                isOut = true;
                continue;
            }
            else if (isOut)
            {
                WindowMessage.ToM();
                int a = 0;
                do
                {
                    if (WindowOpenCV.Test1())
                        break;
                    a++;
                    Thread.Sleep(1000);
                } while (a < 5);
                if (a >= 5)
                {
                    continue;
                }

                WindowMessage.ToServerList();
                Thread.Sleep(500);
                WindowMessage.ToServerList1();
                Thread.Sleep(1000);
                a = 0;
                do
                {
                    if (WindowOpenCV.Test2())
                        break;
                    a++;
                    Thread.Sleep(1000);
                } while (a < 5);
                if (a >= 5)
                {
                    continue;
                }

                WindowMessage.ToServer();
                Thread.Sleep(1000);
                a = 0;
                do
                {
                    if (WindowOpenCV.Test3())
                        break;
                    a++;
                    Thread.Sleep(1000);
                } while (a < 5);
                if (a >= 5)
                {
                    continue;
                }
                WindowMessage.JoinServer();
                isOut = false;
            }
        }
    }
}

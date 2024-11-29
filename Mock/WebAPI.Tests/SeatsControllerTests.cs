using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using WebAPI.Controllers;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.Exceptions;

namespace WebAPI.Tests;

[TestClass()]
public class SeatsControllerTests
{
    DbContextOptions<WebAPIContext> options;
    public SeatsControllerTests()
    {

        
    }


    

    [TestMethod]
    public void ReserveSeatMarche()
    {
        var userId = "2";
        var number = 1;

        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Seat seat = new Seat()
        {
            Number = number,
            ExamenUserId = userId
        };
        serviceMock.Setup(s => s.ReserveSeat(userId, number)).Returns(seat);
        Mock<SeatsController> mockController = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };
        mockController.Setup(c => c.UserId).Returns(userId);
        var controller = mockController.Object;

        var ar = controller.ReserveSeat(1);
        var r = ar.Result as OkObjectResult;
        var s = r.Value as Seat;

        Assert.IsNotNull(r);
        Assert.AreEqual(1, s.Number);
        Assert.AreEqual("2", s.ExamenUserId);



    }

    [TestMethod]
    public void ReserveSeatUnauthorized()
    {
        var userId = "2";
        var number = 1;

        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Seat seat = new Seat()
        {
            Number = number,
            ExamenUserId = userId
        };
        serviceMock.Setup(s => s.ReserveSeat(userId, number)).Throws(new SeatAlreadyTakenException());
        Mock<SeatsController> mockController = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };
        mockController.Setup(c => c.UserId).Returns(userId);
        var controller = mockController.Object;

        var ar = controller.ReserveSeat(1);
        var r = ar.Result as UnauthorizedResult;

        Assert.IsNotNull(r);



    }

    [TestMethod]
    public void ReserveSeatNotFound()
    {
        var userId = "2";
        var number = 1;

        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Seat seat = new Seat()
        {
            Number = number,
            ExamenUserId = userId
        };
        serviceMock.Setup(s => s.ReserveSeat(userId, number)).Throws(new SeatOutOfBoundsException());
        Mock<SeatsController> mockController = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };
        mockController.Setup(c => c.UserId).Returns(userId);
        var controller = mockController.Object;

        var ar = controller.ReserveSeat(number);
        var r = ar.Result as NotFoundObjectResult;

        Assert.IsNotNull(r);
        Assert.AreEqual("Could not find " + number, r.Value);



    }

    [TestMethod]
    public void ReserveSeatBadRequest()
    {
        var userId = "2";
        var number = 1;

        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Seat seat = new Seat()
        {
            Number = number,
            ExamenUserId = userId
        };
        serviceMock.Setup(s => s.ReserveSeat(userId, number)).Throws(new UserAlreadySeatedException());
        Mock<SeatsController> mockController = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };
        mockController.Setup(c => c.UserId).Returns(userId);
        var controller = mockController.Object;

        var ar = controller.ReserveSeat(1);
        var r = ar.Result as BadRequestResult;

        Assert.IsNotNull(r);



    }
}

﻿using RR.Common.IntermediateModels;
namespace RR.App.Service.Interfaces;

public interface IRRApiService
{
    Task Login(Login loginModel);
}
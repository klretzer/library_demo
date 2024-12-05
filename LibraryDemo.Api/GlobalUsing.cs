global using System.Net;
global using System.Reflection;
global using System.Text.Json;
global using FluentValidation;
global using LibraryDemo.Api.Configuration;
global using LibraryDemo.Api.Middleware;
global using LibraryDemo.Api.Modules.Common;
global using LibraryDemo.Core.Commands;
global using LibraryDemo.Core.DTO;
global using LibraryDemo.Core.Mapping;
global using LibraryDemo.Core.Queries;
global using LibraryDemo.Core.Services;
global using LibraryDemo.Core.Validation;
global using LibraryDemo.Data.Contexts;
global using LibraryDemo.Data.Interceptors;
global using LibraryDemo.Data.Repositories;
global using LibraryDemo.Data.Seeding;
global using LibraryDemo.Models.Common;
global using LibraryDemo.Models.Configuration;
global using LibraryDemo.Models.Domain;
global using LibraryDemo.Models.Identity;
global using MediatR;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Http.HttpResults;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.SwaggerGen;
using Poslannik.Framework.Models;
using Poslannik.Api.Services;
using Poslannik.DataBase.Repositories;
using System.Xml.Schema;
using Poslannik.DataBase;


Console.WriteLine("Login:");
var login = Console.ReadLine();

Console.WriteLine("Password:");
var password = Console.ReadLine();

byte[] passwordHash;
byte[] passwordSalt;

PasswordHasher.CreatePasswordHash (password, out passwordHash, out passwordSalt);

Console.WriteLine(BitConverter.ToString(passwordHash));
Console.WriteLine(BitConverter.ToString(passwordSalt));





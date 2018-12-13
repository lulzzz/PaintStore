﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backEnd.Models;

namespace backEnd.Services
{
    public interface IAccountsService
    {
        Accounts AddAccount(Accounts account);
        Accounts EditAccount(Accounts account);
        Accounts RemoveAccount(Accounts account);
    }
}
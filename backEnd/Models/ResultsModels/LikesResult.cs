﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backEnd.Models.ResultsModels
{
    public class LikesResult
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string AvatarImgLink { get; set; }

        public LikesResult(int userId, string name, string avatarImgLink)
        {
            UserId = userId;
            Name = name;
            AvatarImgLink = avatarImgLink;
        }
    }
}
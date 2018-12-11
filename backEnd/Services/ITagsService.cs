﻿using System.Collections.Generic;
using backEnd.Models;

namespace backEnd.Services
{
    public interface ITagsService
    {
        List<Tags> GetTags(int postId);
        Tags AddTag(Tags tag);

    }
}
﻿using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers.Example
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EasyCachingController : ControllerBase
    {
        private IEasyCachingProvider _easyCachingProvider;
        private IEasyCachingProviderFactory _easyCachingProviderFactory;
        public EasyCachingController(IEasyCachingProvider easyCachingProvider,IEasyCachingProviderFactory easyCachingProviderFactory)
        {
            _easyCachingProvider = easyCachingProvider;
            _easyCachingProviderFactory = easyCachingProviderFactory;
        }
        [HttpGet]
        public DateTime GetDateTimeCached()
        {
            if (!_easyCachingProvider.Exists("GetDateTimeCached"))
            {
                _easyCachingProvider.Set("GetDateTimeCached", DateTime.Now, new TimeSpan(0, 0, 5));
            }
            return _easyCachingProvider.Get<DateTime>("GetDateTimeCached").Value;
        }

        [HttpGet]
        public DateTime GetDateTimeCached2()
        {
            var memoryCache=_easyCachingProviderFactory.GetCachingProvider("memoryCache");
            var distributedCache = _easyCachingProviderFactory.GetCachingProvider("redisCache");
            return memoryCache.Get<DateTime>("key").Value;
        }
    }
}

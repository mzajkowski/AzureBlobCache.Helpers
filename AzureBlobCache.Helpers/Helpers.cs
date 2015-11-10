using System;
using System.Net;
using System.Web;
using AzureBlobCache.Helpers.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Core.Cache;

namespace AzureBlobCache.Helpers
{
    public static class Helpers
    {
        public static string GetCropUrl(
                     this IPublishedContent mediaItem,
                     int? width = null,
                     int? height = null,
                     string propertyAlias = Constants.Conventions.Media.File,
                     string cropAlias = null,
                     int? quality = null,
                     ImageCropMode? imageCropMode = null,
                     ImageCropAnchor? imageCropAnchor = null,
                     bool preferFocalPoint = false,
                     bool useCropDimensions = false,
                     bool cacheBuster = true,
                     string furtherOptions = null,
                     ImageCropRatioMode? ratioMode = null,
                     bool upScale = true,
                     bool resolveCdnPath = false)
        {
            var cropUrl = ImageCropperTemplateExtensions.GetCropUrl(mediaItem, width, height, propertyAlias, cropAlias, quality, imageCropMode, imageCropAnchor, preferFocalPoint, useCropDimensions, cacheBuster, furtherOptions, ratioMode, upScale);

            var cachePrefix = "AzureBlobCache_";

            var cacheKey = $"{cachePrefix}{cropUrl}";            

            if (resolveCdnPath)
            {
                var currentDomain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                var absoluteCropPath = $"{currentDomain}{cropUrl}";

                var runtimeCache = ApplicationContext.Current.ApplicationCache.RuntimeCache;

                var cachedItem = runtimeCache.GetCacheItem<CachedImage>(cacheKey);

                if (cachedItem == null)
                {
                    var newCachedImage = new CachedImage {WebUrl = cropUrl};

                    var request = (HttpWebRequest) WebRequest.Create(absoluteCropPath);
                    request.Method = "HEAD";
                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        var responseCode = response.StatusCode;
                        if (responseCode.Equals(HttpStatusCode.OK))
                        {
                            newCachedImage.CacheUrl = response.ResponseUri.AbsoluteUri;

                            runtimeCache.InsertCacheItem<CachedImage>(cacheKey, () => newCachedImage);

                            return response.ResponseUri.AbsoluteUri;
                        }
                    }
                }

                return cachedItem.CacheUrl;
            }

            return cropUrl;
        }
    }
}

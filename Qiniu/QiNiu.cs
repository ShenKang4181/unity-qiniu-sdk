using Qiniu.CDN;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Util;
using System.IO;
using UnityEngine;

public static class QiNiu
{
    public static string ACCESS_KEY;
    public static string SECRET_KEY;
    public static string Bucket;
    public static string URL;

    public static string GetFileHashLocal( string localFile )
    {
        return ETag.CalcHash( localFile );
    }

    public static bool IsFileExists( string key , out string hash )
    {
        var mac = new Mac( ACCESS_KEY , SECRET_KEY );
        var bucketManager = new BucketManager( mac );
        var statResult = bucketManager.Stat( Bucket , key );
        if ( statResult.Code == ( int ) HttpCode.OK )
        {
            hash = statResult.Result.Hash;
            return true;
        }
        else
        {
            hash = string.Empty;
            return false;
        }
    }

    public static void Upload( string localFile , string saveKey )
    {
        Upload( File.ReadAllBytes( localFile ) , saveKey );
    }

    public static void Upload( byte[ ] data , string saveKey )
    {
        var mac = new Mac( ACCESS_KEY , SECRET_KEY );
        var putPolicy = new PutPolicy
        {
            Scope = Bucket + ":" + saveKey
        };
        putPolicy.SetExpires( 3600 );
        var jstr = putPolicy.ToJsonString( );
        var token = Auth.CreateUploadToken( mac , jstr );
        var fu = new FormUploader( );
        var result = fu.UploadData( data , saveKey , token );
        Debug.Log( result );
    }

    public static void CdnRefresh( string key )
    {
        var mac = new Mac( ACCESS_KEY , SECRET_KEY );
        var cdnManager = new CdnManager( mac );
        var urls = new string[ ] { URL + key };
        var result = cdnManager.RefreshUrls( urls );
        Debug.Log( result );
    }
}
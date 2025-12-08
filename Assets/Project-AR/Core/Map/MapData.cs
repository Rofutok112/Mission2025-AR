using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Project_AR.Core.Map
{
    public class Node
    {
        
    }
    
    public static class MapData
    {
        /// <summary>
        /// 緯度と経度から周辺の地理データを取得
        /// </summary>
        /// <param name="lat">緯度</param>
        /// <param name="lon">経度</param>
        /// <param name="radius">取得半径</param>
        /// <param name="apiURL">api用URL</param>
        /// <returns></returns>
        public static async Task<string> GetNodesAsync(double lat, double lon, float radius, string apiURL)
        {
            // APIエンドポイントのURLを構築
            var url = $"{apiURL}/search?lat={lat}&lon={lon}&radius={radius}";
            
            // UnityWebRequestを使用して非同期でデータを取得 usingはリソースの解放のために使用
            using var request = UnityWebRequest.Get(url);
            
            // リクエストを送信し、完了まで待機
            var operation = request.SendWebRequest();
            
            // 非同期で完了を待つ
            while (!operation.isDone)
                await Task.Yield();
            
            // 結果をチェックし、成功ならデータを返す
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            
            Debug.LogError($"API呼び出しエラー: {request.error}");
            return null;
        }
    }
}

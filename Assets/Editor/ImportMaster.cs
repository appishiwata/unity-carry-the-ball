using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

//TODO 全体をリファクタする
//TODO ForEach競合問題 調査

namespace Editor
{
    public class ImportMaster
    {
        [MenuItem("Tools/ImportMasterData")]
        static void ImportMasterData()
        {
            string sheetId = "1zoVn46KQn5LKpHV4DE9WOIpjqQIdXXFoFOFhO3iuhGU";

            {
                // ゲームマスタ
                var asset = ScriptableObject.CreateInstance<GamesData>();
                asset.Items =
                    ImportFromGss<GamesData.GameData>(sheetId, "ゲームマスタ", new string[] { });
                var path = "Assets/Resources/GamesData.asset";
                AssetDatabaseExtension.CreateAssetWithOverwrite(asset, path);
            }
        }
    
        /// <summary>
        /// AssetDatabaseの拡張クラス
        /// </summary>
        public static class AssetDatabaseExtension
        {

            /// <summary>
            /// アセットを上書きで作成する(metaデータはそのまま)
            /// </summary>
            public static void CreateAssetWithOverwrite(UnityEngine.Object asset, string exportPath)
            {
                //アセットが存在しない場合はそのまま作成(metaファイルも新規作成)
                if (!File.Exists(exportPath))
                {
                    AssetDatabase.CreateAsset(asset, exportPath);
                    return;
                }

                //仮ファイルを作るためのディレクトリを作成
                var fileName = Path.GetFileName(exportPath);
                var tmpDirectoryPath = Path.Combine(exportPath.Replace(fileName, ""), "tmpDirectory");
                Directory.CreateDirectory(tmpDirectoryPath);

                //仮ファイルを保存
                var tmpFilePath = Path.Combine(tmpDirectoryPath, fileName);
                AssetDatabase.CreateAsset(asset, tmpFilePath);

                //仮ファイルを既存のファイルに上書き(metaデータはそのまま)
                FileUtil.DeleteFileOrDirectory(exportPath);
                FileUtil.ReplaceFile(tmpFilePath, exportPath);

                //仮ディレクトリとファイルを削除
                AssetDatabase.DeleteAsset(tmpDirectoryPath);

                //データ変更をUnityに伝えるためインポートしなおし
                AssetDatabase.ImportAsset(exportPath);
            }

        }

        static SheetsService OpenSheet()
        {
            string ApplicationName = "Google Sheets API .NET Quickstart";
            GoogleCredential credential;
            using (var stream = new FileStream("Assets/Editor/google_user.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
            }
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        public static Dictionary<string, FieldInfo> DictionaryFromType(Type t)
        {
            var props = t.GetFields();
            var dict = new Dictionary<string, FieldInfo>();
            foreach (var prp in props)
            {
                // object value = prp.GetValue(atype, new object[]{});
                dict.Add(prp.Name, prp);
            }
            return dict;
        }

        //スプレッドシートから取り込み汎用メソッド
        static T[] ImportFromGss<T>(string sheetId, string sheetName, string[] ignoreKeys, bool skipBlankFlg = false)
            where T : new()
        {
            var service = OpenSheet();
            int rowNumber = 1;
            string wRange = string.Format($"{sheetName}", sheetName, rowNumber);
            SpreadsheetsResource.ValuesResource.GetRequest getRequest
                = service.Spreadsheets.Values.Get(sheetId, wRange);
            var values = getRequest.Execute().Values;
            Debug.Log($"{values.Count - 1}件（{sheetName}）");
            var propColumns = values[0].ToList();
            var propToColumnIndex = new Dictionary<string, int>();
            var list = new List<T>();
            var fields = DictionaryFromType(typeof(T));
            fields.ToList().ForEach(prop =>
                propToColumnIndex[prop.Key] = propColumns.FindIndex(c => (string)c == prop.Key));

            ignoreKeys = ignoreKeys.Concat(new[]
            {
                "画像", "BGM", "SE"
            }).ToArray();
        
            var filteredValues = values
                .Where((v, i) => i != 0)
                .Where((v, i) => (!(skipBlankFlg && (string)v.ElementAtOrDefault(0) == "")));
            foreach (var value in filteredValues)
            {
                var item = new T();
                list.Add(item);
                fields.ToList().ForEach(f =>
                {
                    var i = propToColumnIndex[f.Key];
                    var fieldInfo = f.Value;
                    if (i < 0)
                    {
                        if (!ignoreKeys.Contains(f.Key))
                        {
                            Debug.LogError($"不明なColumn:{f.Key}");
                        }
                    }
                    else
                    {
                        var str = (string)value.ElementAtOrDefault(i);
                        if ((fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(System.Int32))
                            && Int32.TryParse(str, out var v))
                        {
                            fieldInfo.SetValue(item, v);
                        }
                        else if (fieldInfo.FieldType == typeof(string))
                        {
                            fieldInfo.SetValue(item, str);
                        }
                        else if ((fieldInfo.FieldType == typeof(bool) || fieldInfo.FieldType == typeof(System.Boolean))
                                 && Int32.TryParse(str, out var v3))
                        {
                            fieldInfo.SetValue(item, v3 != 0);
                        }
                        else if ((fieldInfo.FieldType == typeof(float) || fieldInfo.FieldType == typeof(System.Single))
                                 && Single.TryParse(str, out var v2))
                        {
                            fieldInfo.SetValue(item, v2);
                        }
                    }

                });

            }
            return list.ToArray();
        }
    }
}

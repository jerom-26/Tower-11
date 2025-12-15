using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Supabase")]
    [SerializeField] string supabaseUrl = "https://sfteyxrwvxxzdnxiublf.supabase.co";
    [SerializeField]
    string apiKey =
        "sb_publishable_Z0Y4qpWTIM7bBSJ6L2xhBg_UkmKqqcy";

    [Header("Optional UI")]
    [SerializeField] TMP_Text leaderboardText;

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string username;
        public int best_score;
    }

    [System.Serializable]
    public class LeaderboardWrapper
    {
        public LeaderboardEntry[] rows;
    }

    public void SubmitScore(int score)
    {
        string username = PlayerPrefs.GetString("username", "");
        string error = ValidateUsernameRules(username);

        if (error != null)
        {
            Debug.Log(error);
            return;
        }

        StartCoroutine(SubmitIfHigher(username, score));
    }

    private string ValidateUsernameRules(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return "Username cannot be empty";
        }

        if (username.Length < 3 || username.Length > 20)
        {
            return "Username must be between 3 and 20 characters long.";
        }

        for (int i = 0; i < username.Length; i++)
        {
            char c = username[i];

            bool isLowerLetter = (c >= 'a' && c <= 'z');
            bool isDigit = (c >= '0' && c <= '9');
            bool isUnderscore = (c == '_');

            if (!(isLowerLetter || isDigit || isUnderscore))
                return "Username can only contain lowercase letters (a-z), numbers (0-9), or underscore (_).";
        }

        return null;

    }
     IEnumerator SubmitIfHigher(string username, int score)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection");
            yield break;
        }

        string url = $"{supabaseUrl}/rest/v1/leaderboard?select=best_score&username=eq.{username}&limit=1";
        Debug.Log($"GET {url}");

        var req = UnityWebRequest.Get(url);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("apikey", apiKey);
        req.SetRequestHeader("Authorization", $"Bearer {apiKey}");


        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"❌ BestScore GET failed: {req.responseCode} {req.error} | {req.downloadHandler.text}");
            yield break;
        }


        string raw = req.downloadHandler.text;
        string wrapped = "{\"rows\":" + raw + "}";
        LeaderboardWrapper data = JsonUtility.FromJson<LeaderboardWrapper>(wrapped);

        int existingBest = -1;
        if (data != null && data.rows != null && data.rows.Length > 0) 
        {
            existingBest = data.rows[0].best_score;
        }

        if (score <= existingBest)
        {
            Debug.Log($"Not uploading. score={score} <= existingBest={existingBest}");
            yield break;
        }
        Debug.Log($"Uploading new best. score={score} > existingBest={existingBest}");  
        yield return StartCoroutine(PostScore(username, score));

        yield return StartCoroutine(GetLeaderboard());
    }


    public void LoadLeaderboard() => StartCoroutine(GetLeaderboard());

    IEnumerator PostScore(string username, int score)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection");
            yield break;
        }

        string url = $"{supabaseUrl}/rest/v1/leaderboard?on_conflict=username";
        string json = $"{{\"username\":\"{username}\",\"best_score\":{score}}}";
        Debug.Log($"POST {url}  body={json}");

        var req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("apikey", apiKey);
        req.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Prefer", "resolution=merge-duplicates,return=representation"); // optional

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Score submitted to Supabase: " + req.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"❌ Submit Error: {req.responseCode} {req.error} | {req.downloadHandler.text}");
        }
    }

    IEnumerator GetLeaderboard()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection");
            yield break;
        }

        string url = $"{supabaseUrl}/rest/v1/leaderboard?select=username,best_score&order=best_score.desc&limit=10";
        Debug.Log($"GET {url}");

        var req = UnityWebRequest.Get(url);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("apikey", apiKey);
        req.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"❌ Leaderboard Error: {req.responseCode} {req.error} | {req.downloadHandler.text}");
            yield break;
        }

        string raw = req.downloadHandler.text;
        Debug.Log("Leaderboard raw JSON: " + raw);

        string wrapped = "{\"rows\":" + raw + "}";
        LeaderboardWrapper data = JsonUtility.FromJson<LeaderboardWrapper>(wrapped);

        if (data == null || data.rows == null)
        {
            Debug.LogWarning("⚠️ Leaderboard parse returned null. Showing empty leaderboard.");
            if (leaderboardText != null)
                leaderboardText.text = "Leaderboard\n\n(No entries)";
            yield break;
        }

        if (leaderboardText != null)

        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Leaderboard");
            sb.AppendLine();

            for (int i = 0; i < data.rows.Length; i++)
            {
                var e = data.rows[i];
                string name = string.IsNullOrEmpty(e.username) ? "Player" : e.username;
                sb.AppendLine($"{i + 1}. {name} - {e.best_score}");
            }

            leaderboardText.text = sb.ToString();
            Debug.Log("Formatted Leaderboard:\n" + sb.ToString());

        }
    }
        
}

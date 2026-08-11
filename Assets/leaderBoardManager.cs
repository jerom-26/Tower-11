using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Supabase")]
    [SerializeField]
    private string supabaseUrl = "https://sfteyxrwvxxzdnxiublf.supabase.co";

    [SerializeField]
    private string publishableKey =
        "YOUR_PUBLISHABLE_KEY";

    [Header("Leaderboard UI")]
    [SerializeField]
    private TMP_Text leaderboardText;

    [Header("Current Player UI")]
    [SerializeField]
    private Text bestScoreText;

    [SerializeField]
    private TMP_Text usernameText;


    private const string UsernameKey = "username";
    private const string BestScoreKey = "best_score";

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

    private void Start()
    {
        RefreshPlayerData();
    }

    private void AddSupabaseHeaders(UnityWebRequest request)
    {
        request.SetRequestHeader("apikey", publishableKey);
    }


    public void RefreshPlayerData()
    {
        StartCoroutine(RefreshPlayerDataRoutine());
    }

    private IEnumerator RefreshPlayerDataRoutine()
    {
        string username = GetNormalizedUsername();

        if (!string.IsNullOrEmpty(username))
        {
            yield return StartCoroutine(
                SyncCurrentPlayerBest(username)
            );
        }

        yield return StartCoroutine(GetLeaderboard());
    }


    public void SubmitScore(int score)
    {
        string username = GetNormalizedUsername();

        string error = ValidateUsernameRules(username);

        if (error != null)
        {
            Debug.LogWarning(error);
            return;
        }

        StartCoroutine(
            SubmitIfHigher(username, score)
        );
    }

    private IEnumerator SubmitIfHigher(
        string username,
        int score
    )
    {
        if (Application.internetReachability ==
            NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection");
            yield break;
        }

        string url =
            $"{supabaseUrl}/rest/v1/leaderboard" +
            $"?select=best_score" +
            $"&username=eq.{UnityWebRequest.EscapeURL(username)}" +
            $"&limit=1";

        using UnityWebRequest req =
            UnityWebRequest.Get(url);

        req.downloadHandler =
            new DownloadHandlerBuffer();

        AddSupabaseHeaders(req);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                $"BestScore GET failed: " +
                $"{req.responseCode} {req.error} | " +
                $"{req.downloadHandler.text}"
            );

            yield break;
        }

        string raw = req.downloadHandler.text;

        string wrapped =
            "{\"rows\":" + raw + "}";

        LeaderboardWrapper data =
            JsonUtility.FromJson<LeaderboardWrapper>(
                wrapped
            );

        int existingBest = 0;

        if (
            data != null &&
            data.rows != null &&
            data.rows.Length > 0
        )
        {
            existingBest =
                data.rows[0].best_score;
        }


        // Always synchronize Supabase value locally.
        SetCurrentBest(
            username,
            existingBest
        );

        if (score <= existingBest)
        {
            Debug.Log(
                $"Not uploading. " +
                $"score={score} <= " +
                $"existingBest={existingBest}"
            );
        }
        else
        {
            Debug.Log(
                $"Uploading new best. " +
                $"score={score} > " +
                $"existingBest={existingBest}"
            );

            yield return StartCoroutine(
                PostScore(username, score)
            );
        }

        yield return StartCoroutine(
            GetLeaderboard()
        );
    }


    private IEnumerator SyncCurrentPlayerBest(
        string username
    )
    {
        if (Application.internetReachability ==
            NetworkReachability.NotReachable)
        {
            // If offline, display saved local value.
            int localBest =
                PlayerPrefs.GetInt(
                    BestScoreKey,
                    0
                );

            SetCurrentBest(
                username,
                localBest
            );

            yield break;
        }

        string url =
            $"{supabaseUrl}/rest/v1/leaderboard" +
            $"?select=best_score" +
            $"&username=eq.{UnityWebRequest.EscapeURL(username)}" +
            $"&limit=1";

        using UnityWebRequest req =
            UnityWebRequest.Get(url);

        req.downloadHandler =
            new DownloadHandlerBuffer();

        AddSupabaseHeaders(req);

        yield return req.SendWebRequest();

        if (req.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                $"Player best GET failed: " +
                $"{req.responseCode} {req.error} | " +
                $"{req.downloadHandler.text}"
            );

            yield break;
        }

        string raw =
            req.downloadHandler.text;

        string wrapped =
            "{\"rows\":" + raw + "}";

        LeaderboardWrapper data =
            JsonUtility.FromJson<LeaderboardWrapper>(
                wrapped
            );

        int bestScore = 0;

        if (
            data != null &&
            data.rows != null &&
            data.rows.Length > 0
        )
        {
            bestScore =
                data.rows[0].best_score;
        }

        SetCurrentBest(
            username,
            bestScore
        );

        Debug.Log(
            $"Synced player: " +
            $"{username}, best={bestScore}"
        );
    }

    private IEnumerator PostScore(
        string username,
        int score
    )
    {
        if (Application.internetReachability ==
            NetworkReachability.NotReachable)
        {
            Debug.LogError(
                "No internet connection"
            );

            yield break;
        }

        string url =
            $"{supabaseUrl}/rest/v1/leaderboard" +
            "?on_conflict=username";

        string escapedUsername =
            username.Replace(
                "\"",
                "\\\""
            );

        string json =
            $"{{\"username\":\"{escapedUsername}\"," +
            $"\"best_score\":{score}}}";

        using UnityWebRequest req =
            new UnityWebRequest(
                url,
                "POST"
            );

        req.uploadHandler =
            new UploadHandlerRaw(
                Encoding.UTF8.GetBytes(json)
            );

        req.downloadHandler =
            new DownloadHandlerBuffer();

        AddSupabaseHeaders(req);

        req.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        req.SetRequestHeader(
            "Prefer",
            "resolution=merge-duplicates," +
            "return=representation"
        );

        yield return req.SendWebRequest();

        if (req.result ==
            UnityWebRequest.Result.Success)
        {
            Debug.Log(
                "Score submitted: " +
                req.downloadHandler.text
            );

            // Update local/UI immediately after successful upload.
            SetCurrentBest(
                username,
                score
            );
        }
        else
        {
            Debug.LogError(
                $"Submit Error: " +
                $"{req.responseCode} {req.error} | " +
                $"{req.downloadHandler.text}"
            );
        }
    }

    public void LoadLeaderboard()
    {
        StartCoroutine(GetLeaderboard());
    }

    private IEnumerator GetLeaderboard()
    {
        if (Application.internetReachability ==
            NetworkReachability.NotReachable)
        {
            Debug.LogError(
                "No internet connection"
            );

            yield break;
        }

        string url =
            $"{supabaseUrl}/rest/v1/leaderboard" +
            "?select=username,best_score" +
            "&order=best_score.desc" +
            "&limit=10";

        using UnityWebRequest req =
            UnityWebRequest.Get(url);

        req.downloadHandler =
            new DownloadHandlerBuffer();

        AddSupabaseHeaders(req);

        yield return req.SendWebRequest();

        if (req.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                $"Leaderboard Error: " +
                $"{req.responseCode} {req.error} | " +
                $"{req.downloadHandler.text}"
            );

            yield break;
        }

        string raw =
            req.downloadHandler.text;

        string wrapped =
            "{\"rows\":" + raw + "}";

        LeaderboardWrapper data =
            JsonUtility.FromJson<LeaderboardWrapper>(
                wrapped
            );

        if (
            data == null ||
            data.rows == null
        )
        {
            if (leaderboardText != null)
            {
                leaderboardText.text =
                    "No entries";
            }

            yield break;
        }

        if (leaderboardText == null)
        {
            yield break;
        }

        StringBuilder sb =
            new StringBuilder();

        for (
            int i = 0;
            i < data.rows.Length;
            i++
        )
        {
            LeaderboardEntry entry =
                data.rows[i];

            string playerName =
                string.IsNullOrEmpty(
                    entry.username
                )
                    ? "player"
                    : entry.username;

            string rankText =
                (i + 1)
                .ToString()
                .PadLeft(2);

            string usernameFormatted =
                playerName.PadRight(12);

            string scoreText =
                entry.best_score
                .ToString()
                .PadLeft(3);

            sb.AppendLine(
                $"<color=#FF79C9>" +
                $"{rankText}</color>  " +

                $"<color=#FFF1D6>" +
                $"{usernameFormatted}</color>  " +

                $"<color=#FF79C9>" +
                $"{scoreText}</color>"
            );
        }

        leaderboardText.richText = true;
        leaderboardText.text =
            sb.ToString();
    }

    private void SetCurrentBest(
        string username,
        int bestScore
    )
    {
        PlayerPrefs.SetString(
            UsernameKey,
            username
        );

        PlayerPrefs.SetInt(
            BestScoreKey,
            bestScore
        );

        PlayerPrefs.Save();

        if (bestScoreText != null)
        {
            bestScoreText.text =
                $"BEST {bestScore}";
        }

        if (usernameText != null)
        {
            usernameText.text =
                username;
        }
    }

    private string GetNormalizedUsername()
    {
        string username =
            PlayerPrefs.GetString(
                UsernameKey,
                ""
            );

        username =
            username
                .Trim()
                .ToLowerInvariant();

        PlayerPrefs.SetString(
            UsernameKey,
            username
        );

        PlayerPrefs.Save();

        return username;
    }


    private string ValidateUsernameRules(
        string username
    )
    {
        if (string.IsNullOrEmpty(username))
        {
            return "Username cannot be empty.";
        }

        if (
            username.Length < 3 ||
            username.Length > 20
        )
        {
            return
                "Username must be between " +
                "3 and 20 characters.";
        }

        for (
            int i = 0;
            i < username.Length;
            i++
        )
        {
            char c =
                username[i];

            bool isLowerLetter =
                c >= 'a' && c <= 'z';

            bool isDigit =
                c >= '0' && c <= '9';

            bool isUnderscore =
                c == '_';

            if (
                !isLowerLetter &&
                !isDigit &&
                !isUnderscore
            )
            {
                return
                    "Username can only contain " +
                    "lowercase letters, numbers " +
                    "or underscore.";
            }
        }

        return null;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class KipasSuhuController : MonoBehaviour
{
    [Header("--- DRAG OBJEK KE SINI ---")]
    public mqttManager myManager;
    public Text textSuhu;
    public Text textRPM;
    public GameObject balingBaling;

    [Header("--- SETTING KECEPATAN (UBAH ANGKA INI!) ---")]
    public float boosterGlobal = 100f;
    public float levelRendah = 5f;
    public float levelSedang = 50f;
    public float levelTinggi = 500f;

    [Header("--- ARAH PUTAR ---")]
    public bool gunakanForward = false;

    private float kecepatanSekarang = 0f;

    void Start()
    {
        if (myManager != null) myManager.OnMessageArrived += HandlePesanMasuk;
    }

    void Update()
    {
        if (balingBaling != null && kecepatanSekarang > 0)
        {
            // Kalau "gunakanForward" dicentang, dia pakai Vector3.forward
            // Kalau tidak dicentang, dia pakai Vector3.up (standar)
            Vector3 axis = gunakanForward ? Vector3.forward : Vector3.up;

            float hasilAkhir = kecepatanSekarang * boosterGlobal * Time.deltaTime;
            balingBaling.transform.Rotate(axis * hasilAkhir);
        }
    }

    void HandlePesanMasuk(mqttObj data)
    {
        if (data.topic == "Kipas/Suhu")
        {
            if (float.TryParse(data.msg, out float suhu))
            {
                if (textSuhu != null) textSuhu.text = suhu.ToString("F1") + "°C";

                // LOGIKA PEMILIHAN KECEPATAN
                if (suhu < 20f)
                {
                    kecepatanSekarang = 0f;
                    SetWarnaUI(Color.cyan);
                }
                else if (suhu <= 24.9f)
                {
                    kecepatanSekarang = levelRendah;
                    SetWarnaUI(Color.green);
                }
                else if (suhu <= 30.0f)
                {
                    kecepatanSekarang = levelSedang;
                    SetWarnaUI(new Color(1f, 0.92f, 0.016f)); // Kuning
                }
                else
                {
                    kecepatanSekarang = levelTinggi;
                    SetWarnaUI(Color.red);
                }
            }
        }
        else if (data.topic == "Kipas/Speed" && textRPM != null)
        {
            textRPM.text = data.msg + " RPM";
        }
    }

    void SetWarnaUI(Color c)
    {
        if (textSuhu != null) textSuhu.color = c;
        if (textRPM != null) textRPM.color = c;
    }

    private void OnDestroy()
    {
        if (myManager != null) myManager.OnMessageArrived -= HandlePesanMasuk;
    }
}
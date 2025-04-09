using UnityEngine;

public class LightController : MonoBehaviour
{
    public Transform mesh;
    public Light lightObject;

    Material meshEmissionMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshEmissionMaterial = mesh.GetComponent<MeshRenderer>().materials[1];
    }

    public void ChangeLightColor(Color newColor)
    {
        if (lightObject != null)
        {
            lightObject.color = newColor;
        }
        else
        {
            Debug.LogError("Источник света не задан!");
        }
    }

    public void ChangeLightIntensity(float intensity)
    {
        if (lightObject != null)
        {
            lightObject.intensity = intensity;
        }
        else
        {
            Debug.LogError("Источник света не задан!");
        }
    }

    public void ChangeEmissionColor(Color newColor)
    {
        if (meshEmissionMaterial != null)
        {
            meshEmissionMaterial.SetColor("_EmissionColor", newColor);
            // Включаем Emission, если он выключен
            meshEmissionMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogWarning("Нет материала на объекте.");
        }
    }
}

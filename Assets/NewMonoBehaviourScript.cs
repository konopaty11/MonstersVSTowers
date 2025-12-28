using System;
using UnityEditor;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [ContextMenu("Fix Terrain References")]
    void FixTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (terrain == null) return;

        Debug.Log("Начинаем восстановление Terrain...");

        // 1. Проверяем TerrainData
        if (terrain.terrainData == null)
        {
            Debug.LogError("TerrainData отсутствует!");
            return;
        }

        // 2. Восстанавливаем Terrain Layers
        FixTerrainLayers(terrain.terrainData);

        // 3. Восстанавливаем материал
        FixTerrainMaterial(terrain);

        // 4. Проверяем текстуры
        CheckAndFixTextures(terrain.terrainData);

        Debug.Log("Восстановление завершено");

#if UNITY_EDITOR
        EditorUtility.SetDirty(terrain);
        EditorUtility.SetDirty(terrain.terrainData);
        AssetDatabase.SaveAssets();
#endif
    }

    void FixTerrainLayers(TerrainData terrainData)
    {
        if (terrainData.terrainLayers == null || terrainData.terrainLayers.Length == 0)
        {
            Debug.Log("TerrainLayers отсутствуют, создаем базовые...");
            CreateDefaultTerrainLayers(terrainData);
            return;
        }

        foreach (TerrainLayer layer in terrainData.terrainLayers)
        {
            if (layer == null)
            {
                Debug.LogWarning("Найден null TerrainLayer");
                continue;
            }

            // Проверяем текстуры
            if (layer.diffuseTexture == null)
            {
                Debug.Log($"Слой {layer.name}: diffuseTexture отсутствует");
                layer.diffuseTexture = CreateDefaultTexture(512, 512,
                    new Color(0.4f, 0.6f, 0.3f), "DefaultGrass");
            }

            // Сбрасываем настройки освещения
            layer.metallic = 0f;
            layer.smoothness = 0.1f;

#if UNITY_EDITOR
            EditorUtility.SetDirty(layer);
#endif
        }
    }

    void FixTerrainMaterial(Terrain terrain)
    {
        // Если материал null или белый - создаем новый
        if (terrain.materialTemplate == null ||
            terrain.materialTemplate.name.Contains("Default-Terrain") ||
            terrain.materialTemplate.name.Contains("Hidden/Terrain"))
        {
            Debug.Log("Создаем новый материал для Terrain");

            // Пробуем найти стандартный шейдер Terrain
            Shader terrainShader = Shader.Find("Nature/Terrain/Diffuse");
            if (terrainShader == null)
            {
                terrainShader = Shader.Find("Universal Render Pipeline/Terrain/Lit");
                if (terrainShader == null)
                {
                    terrainShader = Shader.Find("Standard");
                }
            }

            Material newMaterial = new Material(terrainShader);
            newMaterial.name = "Fixed_Terrain_Material";

            // Настраиваем базовые параметры
            if (newMaterial.HasProperty("_Color"))
                newMaterial.SetColor("_Color", Color.white);

            if (newMaterial.HasProperty("_MainTex") && terrain.terrainData.terrainLayers.Length > 0)
            {
                Texture2D tex = terrain.terrainData.terrainLayers[0].diffuseTexture;
                if (tex != null)
                    newMaterial.SetTexture("_MainTex", tex);
            }

            // Отключаем освещение если нужно
            if (newMaterial.HasProperty("_Glossiness"))
                newMaterial.SetFloat("_Glossiness", 0f);
            if (newMaterial.HasProperty("_Metallic"))
                newMaterial.SetFloat("_Metallic", 0f);
            if (newMaterial.HasProperty("_Smoothness"))
                newMaterial.SetFloat("_Smoothness", 0f);

            terrain.materialTemplate = newMaterial;

#if UNITY_EDITOR
            // Сохраняем материал
            string materialPath = "Assets/Terrain_Materials/Fixed_Terrain_Material.mat";
            if (!System.IO.Directory.Exists("Assets/Terrain_Materials"))
                System.IO.Directory.CreateDirectory("Assets/Terrain_Materials");

            AssetDatabase.CreateAsset(newMaterial, materialPath);
            AssetDatabase.SaveAssets();
#endif
        }
        else
        {
            // Проверяем текущий материал
            Debug.Log($"Текущий материал: {terrain.materialTemplate.name}");

            // Сбрасываем параметры материала
            terrain.materialTemplate.SetColor("_Color", Color.white);

            // Проверяем шейдер
            if (terrain.materialTemplate.shader.name.Contains("Hidden"))
            {
                Debug.Log("Материал использует скрытый шейдер, меняем...");
                terrain.materialTemplate.shader = Shader.Find("Nature/Terrain/Diffuse");
            }
        }
    }

    void CheckAndFixTextures(TerrainData terrainData)
    {
        // Проверяем heightmap
        if (terrainData.heightmapTexture == null)
        {
            Debug.LogWarning("Heightmap текстура отсутствует");
            // Можно восстановить из данных высот
        }

        // Проверяем alphamaps
        float[,,] alphaMaps = terrainData.GetAlphamaps(0, 0,
            terrainData.alphamapWidth, terrainData.alphamapHeight);

        if (alphaMaps.GetLength(2) != terrainData.terrainLayers.Length)
        {
            Debug.LogWarning("Размерность alphamaps не соответствует количеству слоев");
        }
    }

    void CreateDefaultTerrainLayers(TerrainData terrainData)
    {
        TerrainLayer[] layers = new TerrainLayer[3];

        // Слой 1: Трава
        layers[0] = new TerrainLayer();
        layers[0].name = "Default_Grass";
        layers[0].diffuseTexture = CreateDefaultTexture(512, 512,
            new Color(0.3f, 0.5f, 0.2f), "GrassTexture");
        layers[0].tileSize = new Vector2(15, 15);
        layers[0].metallic = 0f;
        layers[0].smoothness = 0.1f;

        // Слой 2: Земля
        layers[1] = new TerrainLayer();
        layers[1].name = "Default_Dirt";
        layers[1].diffuseTexture = CreateDefaultTexture(512, 512,
            new Color(0.5f, 0.4f, 0.3f), "DirtTexture");
        layers[1].tileSize = new Vector2(10, 10);
        layers[1].metallic = 0f;
        layers[1].smoothness = 0f;

        // Слой 3: Камни
        layers[2] = new TerrainLayer();
        layers[2].name = "Default_Rock";
        layers[2].diffuseTexture = CreateDefaultTexture(512, 512,
            new Color(0.4f, 0.4f, 0.4f), "RockTexture");
        layers[2].tileSize = new Vector2(20, 20);
        layers[2].metallic = 0.1f;
        layers[2].smoothness = 0.3f;

        terrainData.terrainLayers = layers;

        // Инициализируем alphamaps
        float[,,] alphaMaps = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, 3];
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                alphaMaps[x, y, 0] = 1f; // Трава везде
                alphaMaps[x, y, 1] = 0f;
                alphaMaps[x, y, 2] = 0f;
            }
        }
        terrainData.SetAlphamaps(0, 0, alphaMaps);
    }

    Texture2D CreateDefaultTexture(int width, int height, Color baseColor, string name)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, true);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Добавляем немного шума для текстуры
                float noise = Mathf.PerlinNoise(x * 0.02f, y * 0.02f);
                Color pixel = Color.Lerp(baseColor * 0.8f, baseColor * 1.2f, noise);
                tex.SetPixel(x, y, pixel);
            }
        }

        tex.Apply();
        tex.name = name;

#if UNITY_EDITOR
        // Сохраняем текстуру
        string texturePath = $"Assets/Terrain_Textures/{name}.png";
        if (!System.IO.Directory.Exists("Assets/Terrain_Textures"))
            System.IO.Directory.CreateDirectory("Assets/Terrain_Textures");

        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(texturePath, bytes);
        AssetDatabase.Refresh();

        // Импортируем с правильными настройками
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (importer != null)
        {
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        return AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
#else
        return tex;
#endif
    }

    [ContextMenu("Reset Terrain Completely")]
    void ResetTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (terrain == null) return;

        // Полный сброс TerrainData
        TerrainData newData = new TerrainData();

        // Копируем размеры если были
        if (terrain.terrainData != null)
        {
            newData.size = terrain.terrainData.size;
        }
        else
        {
            newData.size = new Vector3(1000, 600, 1000);
        }

        terrain.terrainData = newData;

        // Создаем новый материал
        Shader terrainShader = Shader.Find("Nature/Terrain/Diffuse");
        if (terrainShader != null)
        {
            Material newMaterial = new Material(terrainShader);
            newMaterial.name = "Reset_Terrain_Material";
            terrain.materialTemplate = newMaterial;
        }

        Debug.Log("Terrain полностью сброшен");

#if UNITY_EDITOR
        EditorUtility.SetDirty(terrain);
        AssetDatabase.SaveAssets();
#endif
    }
}
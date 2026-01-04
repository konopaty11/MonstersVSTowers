using UnityEngine;
using UnityEngine.UIElements;

public class CartridgeController : MonoBehaviour
{
    [SerializeField] GameObject particleSystemPrefab;
    [SerializeField] float particleSystemDelayDestroy;
    [SerializeField] Mesh mesh;

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint _contact = collision.contacts[0];
        GameObject _particleSystemObject = Instantiate
            (
                particleSystemPrefab, 
                _contact.point, 
                Quaternion.LookRotation(_contact.normal)
            );

        Destroy(_particleSystemObject, particleSystemDelayDestroy);
        Destroy(gameObject);

        ParticleSystemRenderer _psRenderer = _particleSystemObject.GetComponent<ParticleSystemRenderer>();
        _psRenderer.renderMode = ParticleSystemRenderMode.Mesh;

        FixMeshSettings(mesh);
    }

    void FixMeshSettings(Mesh mesh)
    {
        // Убедимся, что у меша есть нормали
        if (mesh.normals == null || mesh.normals.Length == 0)
        {
            mesh.RecalculateNormals();
        }

        // Убедимся, что у меша есть UV
        if (mesh.uv == null || mesh.uv.Length == 0)
        {
            // Создаем простые UV
            Vector2[] uvs = new Vector2[mesh.vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
            }
            mesh.uv = uvs;
        }

        // Оптимизируем меш
        mesh.Optimize();
    }
}

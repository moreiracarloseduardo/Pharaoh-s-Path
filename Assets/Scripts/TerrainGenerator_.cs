using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator_ : MonoBehaviour {
    public GameObject outerBlockPrefab;
    public GameObject[] innerBlockPrefabs;

    public int width = 10;
    public int height = 10;

    void Start() {
        GenerateTerrain();
    }

    void GenerateTerrain() {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GameObject blockPrefab;

                // Se estamos na borda do terreno, use o prefab outerBlock
                if (x == 0 || z == 0 || x == width - 1 || z == height - 1) {
                    blockPrefab = outerBlockPrefab;
                } else {
                    // Se não, escolha um prefab aleatório de innerBlockPrefabs
                    blockPrefab = innerBlockPrefabs[Random.Range(0, innerBlockPrefabs.Length)];
                }

                // Instancie o bloco
                Vector3 position = new Vector3(x, 0, z);
                Instantiate(blockPrefab, position, Quaternion.identity, this.transform);
            }
        }
    }
}

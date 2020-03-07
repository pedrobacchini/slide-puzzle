using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<int[]> neighbors = new List<int[]>();
    public List<Block> blocks;
    public int rightPlace = 9;
    public GameObject winText;

    private void Start()
    {
        neighbors.Add(new[] {1, 3});
        neighbors.Add(new[] {0, 2, 4});
        neighbors.Add(new[] {1, 5});
        neighbors.Add(new[] {0, 4, 6});
        neighbors.Add(new[] {1, 3, 5, 7});
        neighbors.Add(new[] {2, 4, 8});
        neighbors.Add(new[] {3, 7});
        neighbors.Add(new[] {4, 6, 8});
        neighbors.Add(new[] {5, 7});

        blocks = GetComponentsInChildren<Block>().ToList();
        blocks.ForEach((block, index) =>
            {
                block.id = index;
                block.neighbors = neighbors[index];
                block.OnMouseDownAsObservable()
                    .Subscribe(_ => TryMoveBlock(block));
            }
        );

        Shuffle();
    }

    //Embaralhar e condição de vitoria
    //Prestar atenção em como embaralhar sem deixar insoluvel
    //Embaralhar como se o jogador estivesse jogando
    // Update is called once per frame

    public void Shuffle()
    {
        winText.SetActive(false);
        var range = Random.Range(1, 100);
        Observable.Range(1, range)
            .Subscribe(_ => ShuffleBlock());
    }

    private void ShuffleBlock()
    {
        var whiteBlockIndex = blocks.FindIndex(block => block.type.Equals(Block.Type.white));
        var whiteBlockNeighbors = blocks[whiteBlockIndex].neighbors;
        var neighborsIndex = whiteBlockNeighbors[Random.Range(0, whiteBlockNeighbors.Length)];
        SwapBlocks(whiteBlockIndex, neighborsIndex);
    }

    private void TryMoveBlock(Block blockToMoving)
    {
        foreach (var neighborsIndex in blockToMoving.neighbors)
        {
            if (blocks[neighborsIndex].type.Equals(Block.Type.normal)) continue;
            var blockToMovingIndex = blocks.FindIndex(block => block.id == blockToMoving.id);
            SwapBlocks(blockToMovingIndex, neighborsIndex);
            if (rightPlace == 9)
                winText.SetActive(true);
            break;
        }
    }

    private void SwapBlocks(int indexBlockA, int indexBlockB)
    {
        var blockA = blocks[indexBlockA];
        var blockB = blocks[indexBlockB];
        //Swap list position
        blocks[indexBlockA] = blockB;
        blocks[indexBlockB] = blockA;
        //Swap unity position
        var position = blockA.transform.position;
        blockA.transform.position = blockB.transform.position;
        blockB.transform.position = position;
        //Swap neighbors
        var neighbor = blockA.neighbors;
        blockA.neighbors = blockB.neighbors;
        blockB.neighbors = neighbor;
        //Update Right Places
        if (indexBlockA == blockA.id)
            rightPlace--;
        else if (indexBlockA == blockB.id)
            rightPlace++;
        if (indexBlockB == blockB.id)
            rightPlace--;
        else if (indexBlockB == blockA.id)
            rightPlace++;
    }
}
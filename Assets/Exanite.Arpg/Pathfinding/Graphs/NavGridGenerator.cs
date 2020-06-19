﻿using System;
using System.Collections;
using UnityEngine;
using Zenject;
using ILogger = Serilog.ILogger;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class NavGridGenerator : MonoBehaviour
    {
        public NavGrid target;

        public bool generateOnStart = false;

        public int sizeX = 10;
        public int sizeY = 10;

        public float nodeSize = 1f;
        public float generationDelay = 0.05f;

        private Coroutine currentTask;

        private ILogger log;

        [Inject]
        public void Inject(ILogger log)
        {
            this.log = log?.ForContext<NavGridGenerator>() ?? throw new ArgumentNullException(nameof(log));
        }

        private void Start()
        {
            if (generateOnStart)
            {
                if (currentTask != null)
                {
                    StopCoroutine(currentTask);

                    log.Information("Grid generation interrupted");
                }

                currentTask = StartCoroutine(GenerateGrid(() => currentTask = null));
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentTask != null)
                {
                    StopCoroutine(currentTask);

                    log.Information("Grid generation interrupted");
                }

                currentTask = StartCoroutine(GenerateGrid(() => currentTask = null));
            }
        }

        private IEnumerator GenerateGrid(Action finishedCallback)
        {
            log.Information("Generating grid of size ({SizeX}, {SizeY})", sizeX, sizeY);

            if (target.nodes != null)
            {
                target.ClearGrid();
            }

            target.nodes = new Node[sizeX, sizeY];

            target.nodeSize = nodeSize;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    var node = new Node
                    {
                        Position = new Vector3(x * nodeSize, 0, y * nodeSize)
                    };

                    if (x > 0)
                    {
                        node.AddConnection(target.nodes[x - 1, y]);
                    }

                    if (y > 0)
                    {
                        node.AddConnection(target.nodes[x, y - 1]);
                    }

                    if (x > 0 && y > 0)
                    {
                        node.AddConnection(target.nodes[x - 1, y - 1]);
                    }

                    if (x > 0 && y < sizeY - 1)
                    {
                        node.AddConnection(target.nodes[x - 1, y + 1]);
                    }

                    if (Physics.OverlapSphere(new Vector3(x * nodeSize, transform.position.y + 0.1f, y * nodeSize), 0).Length > 0
                        || !Physics.Raycast(new Vector3(x * nodeSize, transform.position.y + 1, y * nodeSize), Vector3.down, 2f))
                    {
                        node.Type = NodeType.NonWalkable;
                    }

                    target.nodes[x, y] = node;

                    if (generationDelay > 0)
                    {
                        yield return new WaitForSeconds(generationDelay);
                    }
                }
            }

            target.isGenerated = true;

            log.Information("Grid generation finished");
        }
    }
}

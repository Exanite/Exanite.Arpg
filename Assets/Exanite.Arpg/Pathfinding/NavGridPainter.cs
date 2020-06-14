using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class NavGridPainter : MonoBehaviour
    {
        public NavGrid target;

        private Coroutine currentTask;

        private void Update()
        {
            if (target.isGenerated)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    currentTask = StartCoroutine(PaintGrid());
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    StopCoroutine(currentTask);

                    currentTask = null;
                }
            }
        }

        private IEnumerator PaintGrid()
        {
            HashSet<Node> paintedNodes = new HashSet<Node>();
            NodeType paintAction = NodeType.Walkable;
            bool firstRun = true;

            while (target.isGenerated)
            {
                if (target.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out Node node))
                {
                    if (node != null && !paintedNodes.Contains(node))
                    {
                        if (firstRun == true)
                        {
                            switch (node.Type)
                            {
                                case NodeType.NonWalkable:
                                {
                                    paintAction = NodeType.Walkable;
                                    break;
                                }

                                case NodeType.Walkable:
                                {
                                    paintAction = NodeType.NonWalkable;
                                    break;
                                }
                            }

                            firstRun = false;
                        }

                        node.Type = paintAction;

                        paintedNodes.Add(node);
                    }
                }

                yield return null;
            }
        }
    }
}

namespace WiredSolver
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class WiredSolverInstance
    {
        private Dictionary<Point, WireTransfer> wiredField = new Dictionary<Point, WireTransfer>();

        public List<Point> AddOrUpdateWire(int x, int y, CurrentType newCurrent, WireCurrentTransfer currentTransfer)
        {
            LinkedList<WireTransfer> endResult = new LinkedList<WireTransfer>();
            WireTransfer wireTransfer = this.getWireTransfer(new Point(x, y));
            if (newCurrent != CurrentType.ON)
            {
                if (newCurrent == CurrentType.SENDER)
                {
                    currentTransfer = WireCurrentTransfer.LEFT | WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                    wireTransfer.setCurrentTransfer(currentTransfer);
                    wireTransfer.setCurrent(CurrentType.SENDER);
                    this.updatePowerGrid(wireTransfer, ref endResult);
                }
                else if (newCurrent == CurrentType.OFF)
                {
                    WireTransfer[] inNeighbors = new WireTransfer[4];
                    this.storeNeighbours(wireTransfer, inNeighbors, true);
                    wireTransfer.setCurrent(CurrentType.OFF);
                    wireTransfer.setCurrentTransfer(currentTransfer);
                    this.updatePowerGrid(wireTransfer, ref endResult);
                    for (int i = 0; i < 4; i++)
                    {
                        if (inNeighbors[i] != null)
                        {
                            this.updatePowerGrid(inNeighbors[i], ref endResult);
                        }
                    }
                }
            }
            List<Point> list2 = new List<Point>();
            foreach (WireTransfer transfer2 in endResult)
            {
                if (!list2.Contains(transfer2.location))
                {
                    list2.Add(transfer2.location);
                }
            }
            return list2;
        }

        public void Destroy()
        {
            if (this.wiredField != null)
            {
                this.wiredField.Clear();
            }
            this.wiredField = null;
        }

        private LinkedList<WireTransfer> getLinkedWires(WireTransfer originalSender)
        {
            List<Point> list = new List<Point>(30);
            LinkedList<WireTransfer> list2 = new LinkedList<WireTransfer>();
            LinkedList<WireTransfer> list3 = new LinkedList<WireTransfer>();
            WireTransfer[] inNeighbors = new WireTransfer[4];
            list2.AddFirst(originalSender);
            list3.AddFirst(originalSender);
            list.Add(originalSender.location);
            while (list2.Count > 0)
            {
                WireTransfer toUpdate = list2.First.Value;
                list2.RemoveFirst();
                this.storeNeighbours(toUpdate, inNeighbors, false);
                for (int i = 0; i < 4; i++)
                {
                    if ((inNeighbors[i] != null) && !list.Contains(inNeighbors[i].location))
                    {
                        list.Add(inNeighbors[i].location);
                        list2.AddFirst(inNeighbors[i]);
                        list3.AddFirst(inNeighbors[i]);
                    }
                }
            }
            return list3;
        }

        public WireTransfer getWireTransfer(Point location)
        {
            WireTransfer transfer = null;
            if (!this.wiredField.TryGetValue(location, out transfer))
            {
                transfer = new WireTransfer(location.X, location.Y, WireCurrentTransfer.NONE);
                this.wiredField.Add(transfer.location, transfer);
            }
            return transfer;
        }

        public List<Point> RemoveWire(int x, int y)
        {
            LinkedList<WireTransfer> endResult = new LinkedList<WireTransfer>();
            WireTransfer transfer = this.getWireTransfer(new Point(x, y));
            endResult.AddFirst(transfer);
            if (transfer.isPowered() || (transfer.getTransfer() != WireCurrentTransfer.NONE))
            {
                WireTransfer[] inNeighbors = new WireTransfer[4];
                this.storeNeighbours(transfer, inNeighbors, true);
                transfer.setCurrent(CurrentType.OFF);
                transfer.setCurrentTransfer(WireCurrentTransfer.NONE);
                for (int i = 0; i < 4; i++)
                {
                    if (inNeighbors[i] != null)
                    {
                        this.updatePowerGrid(inNeighbors[i], ref endResult);
                    }
                }
            }
            List<Point> list2 = new List<Point>();
            foreach (WireTransfer transfer2 in endResult)
            {
                if (!list2.Contains(transfer2.location))
                {
                    list2.Add(transfer2.location);
                }
            }
            return list2;
        }

        private void storeNeighbours(WireTransfer toUpdate, WireTransfer[] inNeighbors, bool allneighbours = false)
        {
            WireTransfer transfer;
            int x = toUpdate.location.X;
            int y = toUpdate.location.Y;
            inNeighbors[0] = null;
            inNeighbors[1] = null;
            inNeighbors[2] = null;
            inNeighbors[3] = null;
            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.UP))
            {
                transfer = this.getWireTransfer(new Point(x, y - 1));
                if (allneighbours || transfer.acceptsCurrentFrom(WireCurrentTransfer.DOWN))
                {
                    inNeighbors[0] = transfer;
                }
            }
            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.LEFT))
            {
                transfer = this.getWireTransfer(new Point(x - 1, y));
                if (allneighbours || transfer.acceptsCurrentFrom(WireCurrentTransfer.RIGHT))
                {
                    inNeighbors[1] = transfer;
                }
            }
            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.RIGHT))
            {
                transfer = this.getWireTransfer(new Point(x + 1, y));
                if (allneighbours || transfer.acceptsCurrentFrom(WireCurrentTransfer.LEFT))
                {
                    inNeighbors[2] = transfer;
                }
            }
            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.DOWN))
            {
                transfer = this.getWireTransfer(new Point(x, y + 1));
                if (allneighbours || transfer.acceptsCurrentFrom(WireCurrentTransfer.UP))
                {
                    inNeighbors[3] = transfer;
                }
            }
        }

        private LinkedList<WireTransfer> updatePowerGrid(WireTransfer wireTransfer, ref LinkedList<WireTransfer> endResult)
        {
            LinkedList<WireTransfer> list = this.getLinkedWires(wireTransfer);
            bool powerIsOn = false;
            foreach (WireTransfer transfer in list)
            {
                if (transfer.Current == CurrentType.SENDER)
                {
                    powerIsOn = true;
                    break;
                }
            }
            if (endResult == null)
            {
                endResult = new LinkedList<WireTransfer>();
            }
            int x = 0;
            int y = 0;
            LinkedList<WireTransfer> list2 = new LinkedList<WireTransfer>();
            foreach (WireTransfer transfer in list)
            {
                if (!(powerIsOn || (transfer.getTransfer() != WireCurrentTransfer.NONE)))
                {
                    list2.AddFirst(transfer);
                }
                else if (transfer.setPower(powerIsOn))
                {
                    endResult.AddFirst(transfer);
                }
            }
            foreach (WireTransfer transfer in list2)
            {
                x = transfer.location.X;
                y = transfer.location.Y;
                WireTransfer transfer2 = this.getWireTransfer(new Point(x, y - 1));
                if (transfer2.isPowered() && transfer2.transfersCurrentTo(WireCurrentTransfer.DOWN))
                {
                    if (transfer.setPower(true))
                    {
                        endResult.AddFirst(transfer);
                    }
                }
                else
                {
                    transfer2 = this.getWireTransfer(new Point(x, y + 1));
                    if (transfer2.isPowered() && transfer2.transfersCurrentTo(WireCurrentTransfer.UP))
                    {
                        if (transfer.setPower(true))
                        {
                            endResult.AddFirst(transfer);
                        }
                    }
                    else
                    {
                        transfer2 = this.getWireTransfer(new Point(x - 1, y));
                        if (transfer2.isPowered() && transfer2.transfersCurrentTo(WireCurrentTransfer.RIGHT))
                        {
                            if (transfer.setPower(true))
                            {
                                endResult.AddFirst(transfer);
                            }
                        }
                        else
                        {
                            transfer2 = this.getWireTransfer(new Point(x + 1, y));
                            if (transfer2.isPowered() && transfer2.transfersCurrentTo(WireCurrentTransfer.LEFT))
                            {
                                if (transfer.setPower(true))
                                {
                                    endResult.AddFirst(transfer);
                                }
                            }
                            else if (transfer.setPower(false))
                            {
                                endResult.AddFirst(transfer);
                            }
                        }
                    }
                }
            }
            endResult.AddFirst(wireTransfer);
            return endResult;
        }
    }
}


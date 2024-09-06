using System.Collections;
using System.Collections.Generic;

public interface IConnectable : IColorable
{
    public void Disconnect();
    public void Connect(ConnectableDot dot);
    public void Deselect();
}

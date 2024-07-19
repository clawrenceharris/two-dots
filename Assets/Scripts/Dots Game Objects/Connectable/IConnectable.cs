using System.Collections;
using System.Collections.Generic;

public interface IConnectable
{
    public void Disconnect();
    public void Connect(ConnectableDot dot);
    public void Select();
    public void Deselect();
}

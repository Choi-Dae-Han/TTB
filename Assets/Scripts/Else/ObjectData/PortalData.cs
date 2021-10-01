[System.Serializable]
public class PortalData
{
    public MoveData ExitPMD;
    public ObjectData ExitPOD;

    public PortalData(ObjectData exitPOD = null, MoveData exitPMD = null)
    {
        ExitPOD = exitPOD;
        ExitPMD = exitPMD;
    }
}

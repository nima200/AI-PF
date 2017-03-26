public class TestLeaf2 : LeafNode
{
    public override BehaviorResult Process()
    {
        Print("Testleaf2 proccessing");
        return BehaviorResult.SUCCESS;
    }
}
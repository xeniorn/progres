namespace Progres_Gui_Wpf;

public class ProgresWpfToolTips
{
    public string SearchArgs { get; } = """
                                        optional arguments:
                                        -h, --help            show this help message and exit
                                        -q QUERYSTRUCTURE, --querystructure QUERYSTRUCTURE
                                        query structure file in PDB/mmCIF/MMTF/coordinate format
                                        -l QUERYLIST, --querylist QUERYLIST
                                        text file with one query file path per line
                                        -t TARGETDB, --targetdb TARGETDB
                                        pre-embedded database to search against, either "scope95", "scope40", "cath40", "ecod70", "af21org", "afted" or a file path
                                        -f {guess,pdb,mmcif,mmtf,coords}, --fileformat {guess,pdb,mmcif,mmtf,coords}
                                        file format of the query structure(s), by default guessed from the file extension
                                        -s MINSIMILARITY, --minsimilarity MINSIMILARITY
                                        similarity threshold above which to return hits, default 0.8
                                        -m MAXHITS, --maxhits MAXHITS
                                        maximum number of hits to return, default 100
                                        -d DEVICE, --device DEVICE
                                        device to run on, default is "cpu"
                                        """;
}
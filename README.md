# CrossMatch

Requirements:

GaiaReference is a Windows Forms executable, written in C# and .NET 4.8. The application only runs on TheSky 64-bit unless packaged otherwise. The application runs as an uncertified, standalone application under Windows 10/11.

Installation:

Download PlotReferenceXX.zip to your local drive from the rrskybox->GaiaReferral->publish directory. Extract all contents to a local directory. Launch setup.exe from that directory. Upon completion, an application icon will have been added to the start menu under "TSXToolKit" with the name "Star Tours". This application can be pinned to the Start if desired.

Pre-Usage:

    Browse to IAU Common Names page (https://www.iau.org/public/themes/naming_stars/#n4)
    Select (highlight) the whole displayed table, including the header, but no more than that.
    Copy to clipboard (cntl C).
    Open Excel to new blank worksheet.
    Delete all columns except IAU Name, VMag, RA (J2000), and Dec (J2000)
    Rearrange columns to the order: IAU Name, RA (J2000), Dec (J2000), VMag
    Delete any extra columns or rows
    Save to a csv utf-8 formatted file: File->Save As->Browse->Save As Type->CSV UTF-8 (Comma-Delimited)
    Close Excel
    Run app using List on new csv file

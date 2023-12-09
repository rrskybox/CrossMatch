Cross Match Tool Description

The Cross Match tool assists a TSX user in matching a list of cataloged objects to the nearest Gaia objects, which then can be compiled into a TheSky SDB Text File and input to TheSky as a reference list.  

Cross Match is a Windows Forms executable, written in C# and .NET.  The application runs as an uncertified, standalone application under Windows 7, 8, 10 and 11.  The application requires TheSky 64 (a version compatible with TheSky 32 bit can be generated upon request, no worries).

![image](https://github.com/rrskybox/CrossMatch/assets/40242027/f911827c-1c7b-4571-8023-e88c26ec060d)

Controls:

Map:  Cross reference a single star from the TheSky Star Chart for consideration.  Click on the star and the reference will be projected on the Cross Match star chart.

List:  Cross reference a CSV list of star names from a file in the designated SDB format.  Cross references will be appended to the listings in the original file and the header regenerated for upload into TheSky.

Update RA/Dec:  If checked, the original celestial location (ra/dec) of the star will be replaced with the cross referenced value.

Ignore Magnitudes: If unchecked, the search routine will consider only nearby reference stars with a cataloged magnitude within 2 orders of magnitude of the target star.  If checked, the search routine will consider only proximity in choosing a reference star.  Otherwise, selection will consider both proximity and magnitude.  If no reference star is within both the acceptable proximity and magnitude range, then no reference star will be qualified.

Step:  If selected, the process will pause for each cross reference such that the user can manually select a reference star from the Cross Match star chart if needed.

Next:  Proceed to the next reference when Step is checked.

Abort:  Stop processing and throw away everything.

On Top:  Keep the Cross Match window on top of all other application windows.

Close:  Another way to say done.

Display:

Reference Star Listing:  The list of cataloged stars nearby the target object.

Reference Star Chart:  Visual of nearby stars by position and magnitude (bigger is smaller).

Operation:  


Installation:  

The installation program is located at  https://github.com/rrskybox/CrossMatch in the “publish” folder.  Download the CrossMatch64Buildxxx.zip file.  Extract the zip file to a temporary folder. Run “setup.exe”.  If the Windows security message “Windows protected your pc” pops up, then select “more info” and “Run Anyway”.   You may also have to uninstall any older version, if previously installed.

Upon completion, an application icon will have been added to the start menu under the category "TSX Toolkit" with the name “Cross Match”.

This application can be pinned to the Start if desired.  During operation, Cross Match will expect that TSX is loaded in the default location (User\Documents\Software Bisque etc.
.
Support:  

This application was written as freeware for the public domain and as such is unsupported. The developer wishes you his best and hopes everything works out, but recommends learning C# (it's really not hard and the tools are free from Microsoft) if you find a problem or want to add features.  The source is supplied as a Visual Studio project.

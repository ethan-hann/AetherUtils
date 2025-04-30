// // IconListManager.cs : AetherUtils
// // Copyright (C) 2025  Ethan Hann
// //
// // MIT License
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

using System.Collections;
using JetBrains.Annotations;

namespace AetherUtils.Core.WinForms;

/// <summary>
///     Maintains a list of currently added file extensions
/// </summary>
[UsedImplicitly]
public sealed class IconListManager
{
    private readonly Hashtable _extensionList = new();
    private readonly IconReader.IconSize _iconSize;
    private readonly ArrayList _imageLists = new(); //will hold ImageList objects
    private readonly bool _manageBothSizes; //flag, used to determine whether to create two ImageLists.
    private static readonly char[] Separator = ['.'];

    /// <summary>
    ///     Creates an instance of <c>IconListManager</c> that will add icons to a single <c>ImageList</c> using the
    ///     specified <c>IconSize</c>.
    /// </summary>
    /// <param name="imageList"><c>ImageList</c> to add icons to.</param>
    /// <param name="iconSize">Size to use (either 32 or 16 pixels).</param>
    public IconListManager(ImageList imageList, IconReader.IconSize iconSize)
    {
        // Initialise the members of the class that will hold the image list we're
        // targeting, as well as the icon size (32 or 16)
        _imageLists.Add(imageList);
        _iconSize = iconSize;
    }

    /// <summary>
    ///     Creates an instance of IconListManager that will add icons to two <c>ImageList</c> types. The two
    ///     image lists are intended to be one for large icons, and the other for small icons.
    /// </summary>
    /// <param name="smallImageList">The <c>ImageList</c> that will hold small icons.</param>
    /// <param name="largeImageList">The <c>ImageList</c> that will hold large icons.</param>
    public IconListManager(ImageList smallImageList, ImageList largeImageList)
    {
        //add both our image lists
        _imageLists.Add(smallImageList);
        _imageLists.Add(largeImageList);

        //set flag
        _manageBothSizes = true;
    }

    /// <summary>
    ///     Used internally, adds the extension to the hashtable, so that its value can then be returned.
    /// </summary>
    /// <param name="extension"><c>String</c> of the file's extension.</param>
    /// <param name="imageListPosition">Position of the extension in the <c>ImageList</c>.</param>
    private void AddExtension(string? extension, int imageListPosition)
    {
        if (extension != null)
            _extensionList.Add(extension, imageListPosition);
    }

    /// <summary>
    ///     Called publicly to add a file's icon to the ImageList.
    /// </summary>
    /// <param name="filePath">Full path to the file.</param>
    /// <returns>Integer of the icon's position in the ImageList</returns>
    [UsedImplicitly]
    public int AddFileIcon(string filePath)
    {
        // Check if the file exists, otherwise, throw exception.
        if (!File.Exists(filePath)) throw new FileNotFoundException("File does not exist");

        // Split it down so we can get the extension
        var splitPath = filePath.Split(Separator);
        var extension = (string)splitPath.GetValue(splitPath.GetUpperBound(0))!;

        //Check that we haven't already got the extension, if we have, then
        //return back its index
        if (_extensionList.ContainsKey(extension.ToUpper()))
            return (int)(_extensionList[extension.ToUpper()
                                        ?? throw new InvalidOperationException()] 
                         ?? throw new InvalidOperationException()); //return existing index
        
        // It's not already been added, so add it and record its position.
        var pos = ((ImageList)_imageLists[0]!).Images.Count; //store current count -- new item's index

        if (_manageBothSizes)
        {
            //managing two lists, so add it to small first, then large
            ((ImageList)_imageLists[0]!).Images.Add(IconReader.GetFileIcon(filePath, IconReader.IconSize.Small, false));
            ((ImageList)_imageLists[1]!).Images.Add(IconReader.GetFileIcon(filePath, IconReader.IconSize.Large, false));
        }
        else
        {
            //only doing one size, so use IconSize as specified in _iconSize.
            ((ImageList)_imageLists[0]!).Images.Add(IconReader.GetFileIcon(filePath, _iconSize,
                false)); //add to image list
        }

        AddExtension(extension.ToUpper(), pos); // add to hash table
        return pos;
    }

    /// <summary>
    ///     Clears all <see cref="ImageList" />s that <see cref="IconListManager" /> is managing.
    /// </summary>
    [UsedImplicitly]
    public void ClearLists()
    {
        foreach (ImageList imageList in _imageLists) imageList.Images.Clear(); //clear current image list.

        _extensionList.Clear(); //empty hashtable of entries too.
    }
}
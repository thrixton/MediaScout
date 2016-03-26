﻿using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using MediaScout.Providers;
using System.Xml;
using System.Xml.Serialization;

namespace MediaScoutGUI.GUITypes
{
    public class TVShow : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        
        #endregion

        #region Properties

        public bool IsDeleted = false;

        private bool isUnsortedEpisodeCollection = false;
        public bool IsUnsortedEpisodeCollection
        {
            get { return isUnsortedEpisodeCollection; }
            set
            {
                isUnsortedEpisodeCollection = value;
                NotifyPropertyChanged("IsUnsortedEpisode");
            }
        }

        private bool hasMetadata = false;
        public bool HasMetadata
        {
            get { return hasMetadata; }
            set
            {
                hasMetadata = value;
                NotifyPropertyChanged("HasMetadata");
            }
        }

        private bool metadataChanged = false;
        public bool MetadataChanged 
        {
            get { return metadataChanged ; }
            set
            {
                metadataChanged  = value;
                NotifyPropertyChanged("MetadataChanged");
            }
        }

        private MediaScout.TVShowXML XMLbase = new MediaScout.TVShowXML();
        public MediaScout.TVShowXML XMLBase
        {
            get { return XMLbase; }
            set
            {
                XMLbase = value;
                ID = XMLBase.ID;
                Name = XMLBase.Title;
                Overview = XMLBase.Description;
                FirstAired = XMLBase.FirstAired;
                if (!String.IsNullOrEmpty(XMLBase.Rating))
                    Rating = Decimal.Parse(XMLBase.Rating);
                Genre = new ObservableCollection<MediaScout.Genre>(XMLBase.BuildGenreArrayFromString(XMLBase.Genre));
                Network = new ObservableCollection<MediaScout.Studio>(XMLBase.BuildStudioArrayFromString(XMLBase.Network));
                
                if (!String.IsNullOrEmpty(XMLBase.Actors))
                {
                    Actors = new ObservableCollection<Person>();
                    String[] actorslist = XMLBase.Actors.Split('|');
                    foreach (String actor in actorslist)
                    {
                        if (!String.IsNullOrEmpty(actor))
                        {
                            Person p = new Person(actor, "Actor", null, this, null);
                            Actors.Add(p);
                        }
                    }
                }
                
                NotifyPropertyChanged("XMLBase");
            }
        }

        private MediaScout.TVShowNFO NFObase = new MediaScout.TVShowNFO();
        public MediaScout.TVShowNFO NFOBase
        {
            get { return NFObase; }
            set
            {
                NFObase = value;
                ID = NFOBase.id;
                Name = NFOBase.title;
                Overview = NFOBase.plot;
                FirstAired = NFOBase.aired;
                if(!String.IsNullOrEmpty(NFOBase.rating))
                    Rating = Decimal.Parse(NFOBase.rating);
                Genre = new ObservableCollection<MediaScout.Genre>(XMLBase.BuildGenreArrayFromString(NFOBase.genre));
                Network = new ObservableCollection<MediaScout.Studio>(XMLBase.BuildStudioArrayFromString(NFOBase.studio));

                if (NFOBase.Actors != null && NFOBase.Actors.Count != 0)
                {
                    Actors = new ObservableCollection<Person>();
                    foreach (MediaScout.ActorsNFO actor in NFOBase.Actors)
                    {
                        Person p = new Person(actor.name, "Actor", actor.role, this, null);
                        Actors.Add(p);
                    }
                }
                NotifyPropertyChanged("NFOBase");
            }
        }

        private String id = null;
        public String ID
        {
            get { return id; }
            set
            {
                id = value;

                if (Properties.Settings.Default.SaveMyMoviesMeta)
                    XMLBase.ID = ID;
                if (Properties.Settings.Default.SaveXBMCMeta)
                    NFOBase.id = ID;
                MetadataChanged = true;

                NotifyPropertyChanged("ID");
            }
        }

        private String name = null;
        public string Name
        {
            get { return name; }
            set
            {
                searchterm = name = value;

                if (Properties.Settings.Default.SaveMyMoviesMeta)
                    XMLBase.Title = Name;
                if (Properties.Settings.Default.SaveXBMCMeta)
                    NFOBase.title = Name;
                MetadataChanged = true;

                NotifyPropertyChanged("Name");
            }
        }

        private String overview = null;
        public String Overview
        {
            get { return overview; }
            set
            {
                overview = value;

                if (Properties.Settings.Default.SaveMyMoviesMeta)
                    XMLBase.Overview = Overview;
                if (Properties.Settings.Default.SaveXBMCMeta)
                    NFOBase.plot = Overview;
                MetadataChanged = true;

                NotifyPropertyChanged("Overview");
            }
        }

        private String firstaired = null;
        public string FirstAired
        {
            get { return firstaired; }
            set
            {
                firstaired = value;
                if(!String.IsNullOrEmpty(firstaired))
                    Year = firstaired.Substring(0, 4);

                if (Properties.Settings.Default.SaveMyMoviesMeta)
                    XMLBase.FirstAired = firstaired;
                if (Properties.Settings.Default.SaveXBMCMeta)
                    NFOBase.aired = firstaired;
                MetadataChanged = true;

                NotifyPropertyChanged("FirstAired");
            }
        }

        private Decimal rating = 0;
        public Decimal Rating
        {
            get { return rating; }
            set
            {
                rating = value;

                if (Properties.Settings.Default.SaveMyMoviesMeta)
                    XMLBase.Rating = rating.ToString();
                if (Properties.Settings.Default.SaveXBMCMeta)
                    NFOBase.rating = rating.ToString();
                MetadataChanged = true;

                NotifyPropertyChanged("Rating");
            }
        }

        private ObservableCollection<MediaScout.Genre> genre = null;
        public ObservableCollection<MediaScout.Genre> Genre
        {
            get { return genre; }
            set
            {
                genre = value;

                if (Properties.Settings.Default.SaveMyMoviesMeta)
                    XMLBase.Genre = XMLBase.BuildStringFromGenreArray(Genre);
                if (Properties.Settings.Default.SaveXBMCMeta)
                    NFOBase.genre = XMLBase.BuildStringFromGenreArray(Genre);
                MetadataChanged = true;

                NotifyPropertyChanged("Genre");
            }
        }

        private ObservableCollection<MediaScout.Studio> network = null;
        public ObservableCollection<MediaScout.Studio> Network
        {
            get { return network; }
            set
            {
                network = value;

                if (Properties.Settings.Default.SaveMyMoviesMeta)
                    XMLBase.Network = XMLBase.BuildStringFromStudioArray(Network);
                if (Properties.Settings.Default.SaveXBMCMeta)
                    NFOBase.studio = XMLBase.BuildStringFromStudioArray(Network);
                MetadataChanged = true;

                NotifyPropertyChanged("Network");
            }
        }

        private String year = null;
        public String Year
        {
            get { return year; }
            set
            {
                year = value;
                NotifyPropertyChanged("Year");
            }
        }
                
        public String XMLFile = null;
        public String NFOFile = null;

        private String foldername = null;
        public String Foldername
        {
            get { return foldername; }
            set
            {
                foldername = value;                
                NotifyPropertyChanged("Foldername");
            }
        }

        private String folderpath = null;
        public String Folderpath
        {
            get { return folderpath; }
            set
            {
                folderpath = value;
                XMLFile = folderpath + @"\series.xml";
                NFOFile = folderpath + @"\tvshow.nfo";
                NotifyPropertyChanged("Folderpath");
            }
        }
        
        private String poster = null;
        public String Poster
        {
            get { return poster; }
            set
            {
                poster = value;
                NotifyPropertyChanged("Poster"); }
        }        

        private String backdrop = null;
        public String Backdrop
        {
            get { return backdrop; }
            set
            {
                backdrop = value;
                NotifyPropertyChanged("Backdrop"); }
        }

        private String banner = null;
        public String Banner
        {
            get { return banner; }
            set
            {
                banner = value;
                NotifyPropertyChanged("Banner"); }
        }

        private String searchterm = null;
        public string SearchTerm
        {
            get { return searchterm; }
            set
            {
                searchterm = value;
                NotifyPropertyChanged("SearchTerm");
            }
        }

        private ObservableCollection<Season> seasons;
        public ObservableCollection<Season> Seasons
        {
            get { return seasons;}
            set
            {
                seasons = value;
                NotifyPropertyChanged("Seasons");
            }
        }

        private ObservableCollection<Episode> unsortedEpisodes;
        public ObservableCollection<Episode> UnsortedEpisodes
        {
            get { return unsortedEpisodes; }
            set
            {
                unsortedEpisodes = value;
                NotifyPropertyChanged("UnsortedEpisodes");
            }
        }

        private ObservableCollection<Person> actors;
        public ObservableCollection<Person> Actors
        {
            get { return actors; }
            set
            {
                actors = value;
                MetadataChanged = true;
                NotifyPropertyChanged("Actors");
            }
        }

        #endregion

        #region Image Members

        public bool isPosterLoading = false;
        public bool isBackDropLoading = false;
        public bool isBannerLoading = false;

        public BitmapImage GetImage(TVShowPosterType type)
        {
            BitmapImage bi = null;
            String Filename;
            switch (type)
            {
                case TVShowPosterType.Poster:
                    Filename = Folderpath + @"\folder.jpg";
                    if (File.Exists(Filename))
                    {
                        if( (bi = GetBitmapImage(Filename)) !=null )
                            Poster = Filename;
                    }
                    break;
                case TVShowPosterType.Backdrop:
                    bool success = false;
                    if (Properties.Settings.Default.SaveMyMoviesMeta)
                    {
                        Filename = Folderpath + @"\backdrop.jpg";
                        if (File.Exists(Filename))
                        {
                            if ((bi = GetBitmapImage(Filename)) != null)
                            {
                                Backdrop = Filename;
                                success = true;
                            }
                        }
                    }
                    if (Properties.Settings.Default.SaveXBMCMeta && !success)
                    {
                        Filename = Folderpath + @"\fanart.jpg";
                        if (File.Exists(Filename))
                        {
                            if ((bi = GetBitmapImage(Filename)) != null)
                                Backdrop = Filename;
                        }
                    }
                    break;
                case TVShowPosterType.Banner:
                    Filename = Folderpath + @"\banner.jpg";
                    if (File.Exists(Filename))
                    {
                        if ((bi = GetBitmapImage(Filename)) != null)
                            Banner = Filename;
                    }
                    break;
            }
            return bi;
        }
        public BitmapImage GetBitmapImage(String Filepath)
        {
            BitmapImage bi = null;                        
            try
            {
                bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bi.UriSource = new Uri(Filepath);
                bi.EndInit();
            }
            catch
            {
                bi = null;
            }

            return bi;
        }
        
        #endregion

        #region Load Properties
        
        public void Load()
        {
            bool success = false;
            
            if (Properties.Settings.Default.SaveMyMoviesMeta)
                success = LoadFromXML();
            if (Properties.Settings.Default.SaveXBMCMeta && !success)
                success = LoadFromNFO();

            if (Actors != null && Actors.Count != 0)
                LoadActorsThumb(null);
            
            if(success)
                MetadataChanged = false;
        }
        public bool LoadFromXML()
        {
            bool success = false;
            if (File.Exists(XMLFile))
            {
                try
                {
                    XmlSerializer s = new XmlSerializer(typeof(MediaScout.TVShowXML));
                    TextReader w = new StreamReader(XMLFile);
                    XMLBase = (MediaScout.TVShowXML)s.Deserialize(w);
                    w.Close();

                    HasMetadata = true;
                    //XmlDocument xdoc = new XmlDocument();
                    //xdoc.Load(XMLFile);
                    //XmlNode node = xdoc.DocumentElement;

                    //if(node.SelectSingleNode("id") !=null)
                    //    ID = node.SelectSingleNode("id").InnerText;

                    //if(node.SelectSingleNode("SeriesName") != null)
                    //    Name = node.SelectSingleNode("SeriesName").InnerText;

                    //if (node.SelectSingleNode("Overview") != null)
                    //    Overview = node.SelectSingleNode("Overview").InnerText;

                    //if (node.SelectSingleNode("Actors") != null)
                    //{
                    //    Actors = new ObservableCollection<Person>();
                    //    String[] actorslist = node.SelectSingleNode("Actors").InnerText.Split('|');
                    //    foreach (String actor in actorslist)
                    //    {
                    //        Person p = new Person(actor, "Actor", null, false);
                    //        Actors.Add(p);
                    //    }
                    //}
                    
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            return success;
        }
        public bool LoadFromNFO()
        {
            bool success = false;
            if (File.Exists(NFOFile))
            {
                try
                {
                    XmlSerializer s = new XmlSerializer(typeof(MediaScout.TVShowNFO));
                    TextReader w = new StreamReader(NFOFile);
                    NFOBase = (MediaScout.TVShowNFO)s.Deserialize(w);
                    w.Close();

                    HasMetadata = true;
                    //XmlDocument xdoc = new XmlDocument();
                    //xdoc.Load(NFOFile);
                    //XmlNode node = xdoc.DocumentElement;

                    //if (node.SelectSingleNode("id") != null)
                    //    ID = node.SelectSingleNode("id").InnerText;

                    //if (node.SelectSingleNode("title") != null)
                    //    Name = node.SelectSingleNode("title").InnerText;

                    //if (node.SelectSingleNode("plot") != null)
                    //    Overview = node.SelectSingleNode("plot").InnerText;

                    //Actors = new ObservableCollection<Person>();
                    //XmlNodeList xnl = xdoc.DocumentElement.SelectNodes("actor");
                    //foreach (XmlNode x in xnl)
                    //{
                    //    String ActorName = null;
                    //    String Role = null;
                    //    if (x.SelectSingleNode("name") != null)
                    //        ActorName = x.SelectSingleNode("name").InnerText;
                    //    if (x.SelectSingleNode("role") != null)
                    //        Role = x.SelectSingleNode("role").InnerText;

                    //    Person p = new Person(ActorName, "Actor", Role, false);
                    //    Actors.Add(p);
                    //}

                    success = true;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            return success;
        }
        
        #endregion

        #region Get Search Term And Year Properties

        public String GetSearchTerm(String SearchTerm)
        {
            //SearchTerm = Regex.Replace(SearchTerm, @"\(\w+\)", " ");
            //SearchTerm = Regex.Replace(SearchTerm, @"\[\w+\]", " ");
            //SearchTerm = Regex.Replace(SearchTerm, @"\{\w+\}", " ");
            //SearchTerm = Regex.Replace(SearchTerm, @"\.hdtv|\.2hd|\.ac3|\.x264", " ", RegexOptions.IgnoreCase);
            //SearchTerm = Regex.Replace(SearchTerm, @"\.720p|\.1080p|\.5\.1", " ", RegexOptions.IgnoreCase);
            //SearchTerm = Regex.Replace(SearchTerm, @"\.divx|\.xvid", " ", RegexOptions.IgnoreCase);
            //SearchTerm = Regex.Replace(SearchTerm, @"\.dvdrip|\.dvd|\.rip|\.subs|\.upscaled", " ", RegexOptions.IgnoreCase);
            //SearchTerm = Regex.Replace(SearchTerm, @"\<|\[|\{|\(|\)|\}|\]|\>", " ");
            //SearchTerm = Regex.Replace(SearchTerm, @"\.", " ");
            //SearchTerm = Regex.Replace(SearchTerm, @"\-", " ");
            //SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            //SearchTerm = SearchTerm.Trim();
            SearchTerm = Search.GetSearchTerm(SearchTerm);
            return SearchTerm;
        }
        public String GetSearchTerm()
        {
            SearchTerm = GetSearchTerm(Name);
            return SearchTerm;
        }

        public String GetYearFromFolder()
        {
            if (Year == null)
            {
                MatchCollection mc = Regex.Matches(Name, @"\d{4}");
                if (mc.Count > 0)
                    Year = mc[mc.Count-1].Value;
            }
            return Year;
        }
        public String GetYear()
        {
            GetYearFromFolder();
            if (Year == null)
                Load();
            return Year;
        }
        
        #endregion

        #region Load Season And Unsorted Episodes

        public void LoadSeasons()
        {
            Seasons = new ObservableCollection<Season>();
            if (IsUnsortedEpisodeCollection)
            {
                Season UnsortedSeason = new Season(Folderpath, Name, this);
                Seasons.Add(UnsortedSeason);
                LoadUnsortedEpisodes(UnsortedSeason);
            }
            else
            {
                foreach (DirectoryInfo di in new DirectoryInfo(Folderpath).GetDirectories())
                {
                    Regex rSeasons = new Regex(MediaScoutGUI.Properties.Settings.Default.SeasonFolderName + ".{0,1}([0-9]+)", RegexOptions.IgnoreCase);
                    MatchCollection mc = rSeasons.Matches(di.Name);
                    if (mc.Count > 0 || di.Name == MediaScoutGUI.Properties.Settings.Default.SpecialsFolderName)
                    {
                        GUITypes.Season s = new GUITypes.Season(di.FullName, di.Name, this);
                        Seasons.Add(s);
                    }
                }
            }
        }
        public void LoadUnsortedEpisodes(Season UnsortedSeason)
        {
            UnsortedEpisodes = new ObservableCollection<Episode>();

            List<String> allowedFiles = new List<String>(MediaScoutGUI.Properties.Settings.Default.allowedFileTypes.Split(';'));

            foreach (FileInfo fi in new DirectoryInfo(this.Folderpath).GetFiles())
            {
                if (allowedFiles.Contains(fi.Extension))
                {
                    Episode e = new Episode(fi.FullName, fi.Name, UnsortedSeason);
                    UnsortedEpisodes.Add(e);
                }
            }
        }

        #endregion

        #region Load Actors Thumb
        public void LoadActorsThumb(Person p)
        {
            if (p == null)
                foreach (Person actor in Actors)
                    actor.Thumb = actor.GetImage(Folderpath + "\\.actors");
            else
                p.Thumb = p.GetImage(Folderpath + "\\.actors");
        }
        #endregion
        
        public TVShow(String folderpath, string foldername, bool IsUnsortedEpisodeCollection)
        {
            this.IsUnsortedEpisodeCollection = IsUnsortedEpisodeCollection;
            Folderpath = folderpath;
            Name = Foldername = foldername;
            LoadSeasons();
            MetadataChanged = false;
        }

        public override String ToString()
        {
            return this.Name;
        }
    }
}
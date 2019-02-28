using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    class ProcessSolution
    {
        public static List<string> run(List<string> input)
        {
            var photoNum = Convert.ToInt32(input.ElementAt(0));
            input.Remove(input.ElementAt(0));

            var photos = new List<Photo>();
            int photoCount = 0;
            foreach (var line in input)
            {
                var splitLine = line.Split(' ');
                var currentPhoto = new Photo()
                {
                    ID = photoCount,
                    Orientation = splitLine[0],
                    NumberOfTags = Convert.ToInt32(splitLine[1]),
                    Tags = new List<string>()

                };

                for (int a = 2; a < splitLine.Length; a++)
                {
                    currentPhoto.Tags.Add(splitLine[a]);
                }

                photos.Add(currentPhoto);
                photoCount++;
            }

            var orderedPhotos = photos.OrderByDescending(ph => ph.Tags.Count()).ToList();

            var slides = makeSlides(orderedPhotos);
            var orderedSlides = slides.OrderByDescending(sl => sl.GetTags().Count());

            var slideShow = processSlides(orderedSlides.ToList());

            var output = new List<string>();

            output.Add(slideShow.Count().ToString());

            foreach (var slide in slideShow)
            {
                var IDArray = slide.Photos.Select(ph => ph.ID).ToList();
                var outputIds = string.Empty;
                IDArray.ForEach( id => outputIds += id.ToString() + " " );
                output.Add(outputIds);
            }
            

            return output;
        }

        private static List<Slide> processSlides(List<Slide> slides)
        {
            var slideShow = new List<Slide>();
            for (int i = 0; i < slides.Count()-1; i++)
            {
                var canditate = slides[i];
                var interestFactors = new List<InterestFactor>();

                for (int j = i+1; j < slides.Count(); j++)
                {
                    var potential = slides[j];
                    var factor = canditate.GetTags().Count( tag => potential.GetTags().Contains(tag));

                    var interestFactor = new InterestFactor();
                    interestFactor.Factor = factor;
                    interestFactor.Slide = potential;

                    interestFactors.Add(interestFactor);
                }
               
                var bestInterestFactor = getBestInterestFactor(interestFactors);
                slideShow.Add(canditate);
                slideShow.Add(bestInterestFactor.Slide);
                slides.Remove(canditate);
            }

            return slideShow;
        }

        private static List<Slide> makeSlides(List<Photo> photos)
        {
            var outputSlides = new List<Slide>();
            var verticals = new List<Photo>();
            foreach (var photo in photos)
            {
                var slide = new Slide();
                if (photo.Orientation.ToUpper() == "H")
                {
                    slide.Photos = new List<Photo>(){photo};
                    slide.NumberOfPhotos = 1;
                    outputSlides.Add(slide);
                    continue;
                }

                verticals.Add(photo);
            }

            outputSlides.AddRange(groupVerticals(verticals));
            outputSlides.ForEach(sl => sl.Photos.ForEach(p => p.Tags.ForEach(t => Console.Write(t + " "))));
            return outputSlides;
        }

        private static List<Slide> groupVerticals(List<Photo> verticals)
        {
            var slides = new List<Slide>();
            
            for (int i = 0; i < verticals.Count-1; i++)
            {
                var vertical = verticals[i];
                var matchFactors = new List<MatchFactor>();

                for (int j = i + 1; j < verticals.Count; j++)
                {
                    var potential = verticals[j];
                    var factor = vertical.Tags.Count(tag => potential.Tags.Contains(tag));

                    var matchFactor = new MatchFactor();
                    matchFactor.Factor = factor;
                    matchFactor.Photo = potential;

                    matchFactors.Add(matchFactor);
                }

                var match = getBestMatch(matchFactors); 

                var vertSlide = new Slide();
                vertSlide.NumberOfPhotos = 2;
                vertSlide.Photos = new List<Photo>();
                vertSlide.Photos.Add(vertical);
                vertSlide.Photos.Add(match.Photo);
                slides.Add(vertSlide);

                verticals.Remove(match.Photo);
                 
            };
            
            return slides;
        }

        private static MatchFactor getBestMatch(List<MatchFactor> matchFactors)
        {
            var lowFactor = matchFactors.Min(mf => mf.Factor);

            var lowMFs = matchFactors.Where(mf => mf.Factor == lowFactor);
            var higestTagCount = lowMFs.Max(mf => mf.Photo.Tags.Count());

            var MFWithHighestTagCount = lowMFs.FirstOrDefault(mf => mf.Photo.Tags.Count() == higestTagCount);
            return MFWithHighestTagCount;
        }

        private static InterestFactor getBestInterestFactor(List<InterestFactor> interestFactors)
        {
            var highFactor = interestFactors.Max(mf => mf.Factor);
            var highMFs = interestFactors.Where(mf => mf.Factor == highFactor);
            var higestTagCount = highMFs.Max(mf => mf.Slide.GetTags().Count());
            var IFWithHighestTagCount = highMFs.FirstOrDefault(mf => mf.Slide.GetTags().Count() == higestTagCount);
            return IFWithHighestTagCount;
        }
    }
}

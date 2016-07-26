# Novel Open Variable Annotator
NovA is a tool for annotating and analyzing behaviours in social interactions. It allows to load data recorded with the SSI Framework, as well as from external sources. It further makes use of SSI for semi-automated labeling of behaviours for example by automatically detecting specific gestures from a Kinect (or Kinect 2) Sensor or facial expressions from video.

![alt tag](http://hcm-lab.de/projects/ssi/wp-content/uploads/2016/07/nova.png)


NovA has been completly reworked with more advanced annotation features. It now allows framewise labeling for a more precise coding experience, and continuous annotations for labeling e.g emotions or social attitudes (see picture below). The interface is more customizable than the last version and allows loading and labeling data of multiple persons.

The Annotation format can easily be imported in other tools, like ELAN or Excel. NovA further supports the Import of Discrete Annotation files from ELAN and ANVIL for a seamless workflow. Annotations further can directly be transformed into SSI samplelists for training models.

![alt tag](http://hcm-lab.de/projects/ssi/wp-content/uploads/2016/07/cont.png)

The new reworked version is now online for Download.

FAQ:
-NovA runs on Windows only
-It is Software under development and is provided “as is”. If you run into any problems or find bugs feel free to open an issue
-If you want to open a video but it doesn’t show, please make sure you installed the according Videocodec on your System. E.g. The K-Lite Codec Pack might be a good solution for most missing codecs.
-An example SSI Pipeline for recording a wide range of interessting features will be provided in the next update (as it existed for the old version, but updated for Kinect 2, and some more new features 🙂 ).


##Publications:

Tobias Baur, Gregor Mehlmann, Ionut Damian, Florian Lingenfelser, Johannes Wagner, Birgit Lugrin, Elisabeth André, Patrick Gebahard, “Context-Aware Automated Analysis and Annotation of Social Human-Agent Interactions” in ACM Transactions on Interactive Intelligent Systems (TiiS) 5.2, 2015

<pre><code>

@article{
  title={Context-Aware Automated Analysis and Annotation of Social Human-Agent Interactions},
  author={Baur, Tobias and Mehlmann, Gregor and Damian, Ionut and Lingenfelser, Florian and Wagner, Johannes and Lugrin, Birgit and Andr{\'e}, Elisabeth and Gebhard, Patrick},
  journal={ACM Transactions on Interactive Intelligent Systems (TiiS)},
  volume={5},
  number={2},
  pages={11},
  year={2015},
  publisher={ACM}
}

</code></pre>
Tobias Baur, Ionut Damian, Florian Lingenfelser, Johannes Wagner and Elisabeth André, “NovA: Automated Analysis of Nonverbal Signals in Social Interactions” in Human Behavior Understanding, LNCS 8212, 2013.

<pre><code>
@incollection{
year={2013},
isbn={978-3-319-02713-5},
booktitle={Human Behavior Understanding},
volume={8212},
series={Lecture Notes in Computer Science},
editor={Salah, AlbertAli and Hung, Hayley and Aran, Oya and Gunes, Hatice},
doi={10.1007/978-3-319-02714-2_14},
title={NovA: Automated Analysis of Nonverbal Signals in Social Interactions},
url={http://dx.doi.org/10.1007/978-3-319-02714-2_14},
publisher={Springer International Publishing},
author={Baur, Tobias and Damian, Ionut and Lingenfelser, Florian and Wagner, Johannes and André, Elisabeth},
pages={160-171}}

</code></pre>
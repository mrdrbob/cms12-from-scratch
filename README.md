# CMS From Scratch

This repository is to support the CMS from Scratch series of videos I've been creating on Youtube. You can follow along with me in this repo. Except for the first few videos, each episode will be tagged with a commit. That tag will point the to STARTING point of the video. This repo did not exist for the first three videos, so for those, you are on your own.

Below are the episodes, and corresponding tags (should they exist):


|Episode Name|Link|Github Tag|
|--|--|--|
|01 - Optimizely CMS 12 Project Setup|https://youtu.be/mdQYec2JhQA||
|02 - Content Modeling in Optimizely CMS 12|https://youtu.be/XxHBlCZrvEE||
|03 - Rendering Content in Optimizely CMS 12|https://youtu.be/DTvoE4xCVSA||
|04 - Working with Content in Optimizely CMS 12|https://youtu.be/2IfrEB9IbBA||
|05 - Events and Scheduled Jobs in Optimizely CMS 12|https://youtu.be/aPx2klRTnvs|`episode/05-events-and-jobs`|

## Disclaimer

No code in this repository should be considered ready for production. Everything here is for education purposes and might not represent best practice. Use anything here at your own risk.

## Content Index

Attached to this project is `Blend.ContentIndex`, which is a simple indexing system meant for demonstration purposes only. It stores bits of data about content in rows in a SQL database. A simplified, conceptual exapmle might be something like:

|Page|Attribute|Value|
|--|--|--|
|Homepage|ContentType|Homepage|
|Homepage|ContentType|AbstractContentPage|
|About Us|ContentType|TextPage|
|About Us|ContentType|AbstractContentPage|
|About Us|Category|Informational|
|About Us|Category|Historical|

It supports querying against this collection of values to find pages that match certain criteria. Again, this index is purely for demonstration purposes, has no caching, and is not suitable for any production workload. Consider Optimizely Search & Navigation as an alternative.

## License

This code is licensed under the MIT license.

## Theme

The theme is borrowed from [Start Bootstrap - Clean Blog Jekyll - Official Jekyll Version](https://github.com/StartBootstrap/startbootstrap-clean-blog-jekyll), also under the MIT License.


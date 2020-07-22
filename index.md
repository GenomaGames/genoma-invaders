---
layout: article
---

# Genoma Invaders

Genoma Invaders is a [Fixed shooter](https://en.wikipedia.org/wiki/Category:Fixed_shooters) from the [Shoot 'em up](https://en.wikipedia.org/wiki/Shoot_%27em_up#Fixed_shooters) genre game developed to teach the basics for game development in Unity which development is explained through a series of tutorials.

{% for tutorial in site.tutorials %}
1. [{{ tutorial.title }}]({{ tutorial.url | prepend: site.baseurl }})
{% endfor %}
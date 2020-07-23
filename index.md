---
layout: article
---

# Genoma Invaders

Genoma Invaders is a [Fixed shooter](https://en.wikipedia.org/wiki/Category:Fixed_shooters) from the [Shoot 'em up](https://en.wikipedia.org/wiki/Shoot_%27em_up) genre game developed to teach the basics for game development in Unity which development is explained through a series of tutorials.

{% for tutorial in site.tutorials %}
1. [{{ tutorial.name }}]({{ tutorial.url | relative_url }})

   {{ tutorial.excerpt }}
{% endfor %}

{% if site.github.environment == 'development' %}

{% for post in site.posts | where: 'draft', true %}
1. [{{ post.name }}]({{ post.url | relative_url }})

   {{ post.excerpt }}
{% endfor %}

{% endif %}
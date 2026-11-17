FROM php:8.2-apache

RUN a2enmod rewrite headers

COPY docker/v14/web/apache-vhost.conf /etc/apache2/sites-available/000-default.conf

WORKDIR /var/www/html

# Copy client assets and directory structure
COPY Retros-Portable/2007-v14/2007-v14/Server/www/c_images /var/www/html/c_images
COPY Retros-Portable/2007-v14/2007-v14/Server/www/dcr /var/www/html/dcr
COPY Retros-Portable/2007-v14/2007-v14/Server/www/gamedata /var/www/html/gamedata
COPY Retros-Portable/2007-v14/2007-v14/Server/www/public /var/www/html/public
COPY Retros-Portable/2007-v14/2007-v14/Server/www/web-gallery /var/www/html/web-gallery
COPY Retros-Portable/2007-v14/2007-v14/Server/www/crossdomain.xml /var/www/html/crossdomain.xml
COPY Retros-Portable/2007-v14/2007-v14/Server/www/boo.gif /var/www/html/boo.gif

# Copy .htaccess, dynamic index.php and external_variables.php
COPY Retros-Portable/2007-v14/2007-v14/Server/www/.htaccess /var/www/html/.htaccess
COPY docker/v14/web/index.php /var/www/html/index.php
COPY docker/v14/web/external_variables.php /var/www/html/external_variables.php

COPY docker/v14/web/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]

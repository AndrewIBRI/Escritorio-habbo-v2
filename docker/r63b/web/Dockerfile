FROM php:5.6-apache

# Fix Debian archive repositories for legacy PHP 5.6
RUN sed -i 's/deb.debian.org/archive.debian.org/g' /etc/apt/sources.list && \
    sed -i 's|security.debian.org|archive.debian.org/|g' /etc/apt/sources.list && \
    sed -i '/stretch-updates/d' /etc/apt/sources.list || true

RUN docker-php-ext-install mysql mysqli pdo pdo_mysql || true
RUN a2enmod rewrite

# Copy Apache configuration
COPY docker/r63b/web/apache-vhost.conf /etc/apache2/sites-available/000-default.conf

# Copy CMS files
COPY Retros-Portable/2012-r63b/2012-r63b/Server/htdocs/ /var/www/html/
COPY docker/r63b/web/crossdomain.xml /var/www/html/crossdomain.xml

COPY docker/r63b/web/entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

EXPOSE 80

ENTRYPOINT ["/entrypoint.sh"]

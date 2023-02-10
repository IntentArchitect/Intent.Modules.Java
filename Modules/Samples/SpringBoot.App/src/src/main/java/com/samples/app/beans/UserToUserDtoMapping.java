package com.samples.app.beans;
import com.samples.app.application.models.UserDto;
import com.samples.app.domain.models.User;

import org.modelmapper.PropertyMap;

public class UserToUserDtoMapping extends PropertyMap<User, UserDto> {
    protected void configure() {
      map().setEmailAddress(source.getEmail());
      map().setPhone(source.getPhoneNumber());
    }
}
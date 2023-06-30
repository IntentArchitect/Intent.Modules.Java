package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.data;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;
import java.util.Optional;
import java.util.UUID;
import org.springframework.data.repository.query.Param;
import za.myorg.mypackage.Order;
import org.springframework.data.jpa.repository.Query;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.data.projections.OrderQueryProjection;

@IntentMerge
public interface OrderRepository extends JpaRepository<Order, UUID> {

    @Query("select " +
                "order.number as number, " +
                "order.id as id " +
                "from Order order join order.orderItems orderItem " +
                "where orderItem.description = :description")
        Optional<OrderQueryProjection> getOrderByDescription(@Param("description") String description);
}